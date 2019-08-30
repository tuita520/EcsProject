using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Frame.Core.Base;
using RDLog;
using Server.Core.Network.Helper;

namespace Server.Core.Network.TCP
{
    public class TCPChannel : AChannel
    {
        private readonly Socket _socket;
        private readonly SocketAsyncEventArgs _writeArgs = new SocketAsyncEventArgs();
        private readonly SocketAsyncEventArgs _readArgs = new SocketAsyncEventArgs();

        private readonly CircularBuffer _sendBuffer = new CircularBuffer();
        private readonly CircularBuffer _recvBuffer = new CircularBuffer();

        private readonly MemoryStream _memoryStream;
        
        private bool isSending;

        private bool isRecving;

        private bool isConnected;

        private readonly PacketParser _parser;

        private readonly byte[] packetSizeCache;

        public TCPChannel(TCPService service, IPEndPoint ipEndPoint) : base(service, ChannelType.Connect)
        {
            int packetSize = service.PacketSizeLength;
            packetSizeCache = new byte[packetSize];
            _memoryStream = service.MemoryStreamManager.GetStream("message", ushort.MaxValue);

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.NoDelay = true;
            _parser = new PacketParser(packetSize, _recvBuffer, _memoryStream);
            _writeArgs.Completed += OnComplete;
            _readArgs.Completed += OnComplete;

            RemoteAddress = ipEndPoint;

            isConnected = false;
            isSending = false;
        }

        public TCPChannel(TCPService service, Socket socket) : base(service, ChannelType.Accept)
        {
            int packetSize = service.PacketSizeLength;
            packetSizeCache = new byte[packetSize];
            _memoryStream = service.MemoryStreamManager.GetStream("message", ushort.MaxValue);

            _socket = socket;
            _socket.NoDelay = true;
            _parser = new PacketParser(packetSize, _recvBuffer, _memoryStream);
            _writeArgs.Completed += OnComplete;
            _readArgs.Completed += OnComplete;

            RemoteAddress = (IPEndPoint) socket.RemoteEndPoint;

            isConnected = true;
            isSending = false;
        }

        public bool IsSending => isSending;

        private TCPService GetService()
        {
            return (TCPService) Service;
        }

        private void OnComplete(object sender, SocketAsyncEventArgs eventArgs)
        {
            switch (eventArgs.LastOperation)
            {
                case SocketAsyncOperation.Connect:
                    MainThreadSynchronizationContext.Inst.Post(OnConnectComplete, eventArgs);
                    break;
                case SocketAsyncOperation.Receive:
                    MainThreadSynchronizationContext.Inst.Post(OnRecvComplete, eventArgs);
                    break;
                case SocketAsyncOperation.Send:
                    MainThreadSynchronizationContext.Inst.Post(OnSendComplete, eventArgs);
                    break;
                case SocketAsyncOperation.Disconnect:
                    MainThreadSynchronizationContext.Inst.Post(OnDisconnectComplete, eventArgs);
                    break;
                default:
                    throw new Exception($"socket error: {eventArgs.LastOperation}");
            }
        }

        private void OnDisconnectComplete(object state)
        {
            SocketAsyncEventArgs eventArgs = (SocketAsyncEventArgs) state;
            OnError((int) eventArgs.SocketError);
        }

        public override void Send(MemoryStream stream)
        {
            if (IsDisposed)
            {
                throw new Exception("TCPChannel已经被Dispose, 不能发送消息");
            }

            switch (GetService().PacketSizeLength)
            {
                case Packet.PacketSizeLength4:
                    if (stream.Length > ushort.MaxValue * 16)
                    {
                        throw new Exception($"send packet too large: {stream.Length}");
                    }

                    packetSizeCache.WriteTo(0, (int) stream.Length);
                    break;
                case Packet.PacketSizeLength2:
                    if (stream.Length > ushort.MaxValue)
                    {
                        throw new Exception($"send packet too large: {stream.Length}");
                    }

                    packetSizeCache.WriteTo(0, (ushort) stream.Length);
                    break;
                default:
                    throw new Exception("packet size must be 2 or 4!");
            }

            _sendBuffer.Write(packetSizeCache, 0, packetSizeCache.Length);
            _sendBuffer.Write(stream);

            GetService().MarkNeedStartSend(Id);
        }
        private void OnSendComplete(object state)
        {
            if (_socket == null)
            {
                return;
            }

            SocketAsyncEventArgs eventArgs = (SocketAsyncEventArgs) state;

            if (eventArgs.SocketError != SocketError.Success)
            {
                OnError((int) eventArgs.SocketError);
                return;
            }

            if (eventArgs.BytesTransferred == 0)
            {
                OnError(NetworkErrorCode.PeerDisconnect);
                return;
            }

            _sendBuffer.FirstIndex += eventArgs.BytesTransferred;
            if (_sendBuffer.FirstIndex == _sendBuffer.ChunkSize)
            {
                _sendBuffer.FirstIndex = 0;
                _sendBuffer.RemoveFirst();
            }

            StartSend();
        }

        public void StartSend()
        {
            if (!isConnected)
            {
                return;
            }

            // 没有数据需要发送
            if (_sendBuffer.Length == 0)
            {
                isSending = false;
                return;
            }

            isSending = true;

            int sendSize = _sendBuffer.ChunkSize - _sendBuffer.FirstIndex;
            if (sendSize > _sendBuffer.Length)
            {
                sendSize = (int) _sendBuffer.Length;
            }

            SendAsync(_sendBuffer.First, _sendBuffer.FirstIndex, sendSize);
        }

        private void SendAsync(byte[] buffer, int offset, int count)
        {
            try
            {
                _writeArgs.SetBuffer(buffer, offset, count);
            }
            catch (Exception e)
            {
                throw new Exception($"socket set buffer error: {buffer.Length}, {offset}, {count}", e);
            }

            if (_socket.SendAsync(_writeArgs))
            {
                return;
            }

            OnSendComplete(_writeArgs);
        }

        private void OnRecvComplete(object state)
        {
            if (_socket == null)
            {
                return;
            }

            SocketAsyncEventArgs eventArgs = (SocketAsyncEventArgs) state;

            if (eventArgs.SocketError != SocketError.Success)
            {
                OnError((int) eventArgs.SocketError);
                return;
            }

            if (eventArgs.BytesTransferred == 0)
            {
                OnError(NetworkErrorCode.PeerDisconnect);
                return;
            }

            _recvBuffer.LastIndex += eventArgs.BytesTransferred;
            if (_recvBuffer.LastIndex == _recvBuffer.ChunkSize)
            {
                _recvBuffer.AddLast();
                _recvBuffer.LastIndex = 0;
            }

            // 收到消息回调
            while (true)
            {
                try
                {
                    if (!_parser.Parse())
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e);
                    OnError(NetworkErrorCode.SocketError);
                    return;
                }

                try
                {
                    OnRead(_parser.GetPacket());
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }

            if (_socket == null)
            {
                return;
            }

            StartRecv();
        }

        private void StartRecv()
        {
            int size = _recvBuffer.ChunkSize - _recvBuffer.LastIndex;
            RecvAsync(_recvBuffer.Last, _recvBuffer.LastIndex, size);
        }

        private void RecvAsync(byte[] buffer, int offset, int count)
        {
            try
            {
                _readArgs.SetBuffer(buffer, offset, count);
            }
            catch (Exception e)
            {
                throw new Exception($"socket set buffer error: {buffer.Length}, {offset}, {count}", e);
            }

            if (_socket.ReceiveAsync(_readArgs))
            {
                return;
            }

            OnRecvComplete(_readArgs);
        }


        private void OnConnectComplete(object state)
        {
            if (_socket == null)
            {
                return;
            }

            SocketAsyncEventArgs eventArgs = (SocketAsyncEventArgs) state;

            if (eventArgs.SocketError != SocketError.Success)
            {
                OnError((int) eventArgs.SocketError);
                return;
            }

            eventArgs.RemoteEndPoint = null;
            isConnected = true;
        }

        public override void Start()
        {
            if (!isRecving)
            {
                isRecving = true;
                StartRecv();
            }
            GetService().MarkNeedStartSend(Id);
        }
    }
}