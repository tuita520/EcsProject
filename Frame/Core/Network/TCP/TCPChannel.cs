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
        private Socket socket;
        private SocketAsyncEventArgs writeArgs = new SocketAsyncEventArgs();
        private SocketAsyncEventArgs readArgs = new SocketAsyncEventArgs();

        private readonly CircularBuffer sendBuffer = new CircularBuffer();
        private readonly CircularBuffer recvBuffer = new CircularBuffer();

        private bool isSending;

        private bool isRecving;

        private bool isConnected;

        private readonly PacketParser parser;

        private readonly byte[] packetSizeCache;

        public TCPChannel(AService service, IPEndPoint ipEndPoint) : base(service, ChannelType.Connect)
        {
            int packetSize = service.PacketSizeLength;
            packetSizeCache = new byte[packetSize];
            this.memoryStream = service.MemoryStreamManager.GetStream("message", ushort.MaxValue);

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.NoDelay = true;
            parser = new PacketParser(packetSize, recvBuffer, this.memoryStream);
            sendArgs.Completed += OnComplete;
            recvArgs.Completed += OnComplete;

            RemoteAddress = ipEndPoint;

            isConnected = false;
            isSending = false;
        }

        public TCPChannel(AService service, Socket socket) : base(service, ChannelType.Accept)
        {
            int packetSize = service.PacketSizeLength;
            packetSizeCache = new byte[packetSize];
            this.memoryStream = service.MemoryStreamManager.GetStream("message", ushort.MaxValue);

            this.socket = socket;
            this.socket.NoDelay = true;
            parser = new PacketParser(packetSize, recvBuffer, this.memoryStream);
            sendArgs.Completed += OnComplete;
            recvArgs.Completed += OnComplete;

            RemoteAddress = (IPEndPoint) socket.RemoteEndPoint;

            isConnected = true;
            isSending = false;
        }

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

            sendBuffer.Write(packetSizeCache, 0, packetSizeCache.Length);
            sendBuffer.Write(stream);

            GetService().MarkNeedStartSend(Id);
        }
        private void OnSendComplete(object state)
        {
            if (socket == null)
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

            sendBuffer.FirstIndex += eventArgs.BytesTransferred;
            if (sendBuffer.FirstIndex == sendBuffer.ChunkSize)
            {
                sendBuffer.FirstIndex = 0;
                sendBuffer.RemoveFirst();
            }

            StartSend();
        }

        private void StartSend()
        {
            if (!isConnected)
            {
                return;
            }

            // 没有数据需要发送
            if (sendBuffer.Length == 0)
            {
                isSending = false;
                return;
            }

            isSending = true;

            int sendSize = sendBuffer.ChunkSize - sendBuffer.FirstIndex;
            if (sendSize > sendBuffer.Length)
            {
                sendSize = (int) sendBuffer.Length;
            }

            SendAsync(sendBuffer.First, sendBuffer.FirstIndex, sendSize);
        }

        private void SendAsync(byte[] buffer, int offset, int count)
        {
            try
            {
                writeArgs.SetBuffer(buffer, offset, count);
            }
            catch (Exception e)
            {
                throw new Exception($"socket set buffer error: {buffer.Length}, {offset}, {count}", e);
            }

            if (socket.SendAsync(writeArgs))
            {
                return;
            }

            OnSendComplete(writeArgs);
        }

        private void OnRecvComplete(object state)
        {
            if (socket == null)
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

            recvBuffer.LastIndex += eventArgs.BytesTransferred;
            if (recvBuffer.LastIndex == recvBuffer.ChunkSize)
            {
                recvBuffer.AddLast();
                recvBuffer.LastIndex = 0;
            }

            // 收到消息回调
            while (true)
            {
                try
                {
                    if (!parser.Parse())
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
                    OnRead(parser.GetPacket());
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }

            if (socket == null)
            {
                return;
            }

            StartRecv();
        }

        private void StartRecv()
        {
            int size = recvBuffer.ChunkSize - recvBuffer.LastIndex;
            RecvAsync(recvBuffer.Last, recvBuffer.LastIndex, size);
        }

        private void RecvAsync(byte[] buffer, int offset, int count)
        {
            try
            {
                readArgs.SetBuffer(buffer, offset, count);
            }
            catch (Exception e)
            {
                throw new Exception($"socket set buffer error: {buffer.Length}, {offset}, {count}", e);
            }

            if (socket.ReceiveAsync(readArgs))
            {
                return;
            }

            OnRecvComplete(readArgs);
        }


        private void OnConnectComplete(object state)
        {
            if (socket == null)
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

            StartConnect();
        }

        public override void StartConnect()
        {
            if (!isConnected)
            {
                ConnectAsync(RemoteAddress);
                return;
            }

            if (!isRecving)
            {
                isRecving = true;
                StartRecv();
            }

            GetService().MarkNeedStartSend(Id);
        }

        private void ConnectAsync(IPEndPoint ipEndPoint)
        {
            sendArgs.RemoteEndPoint = ipEndPoint;
            if (socket.ConnectAsync(sendArgs))
            {
                return;
            }

            OnConnectComplete(sendArgs);
        }


    }
}