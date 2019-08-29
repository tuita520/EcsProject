using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Frame.Core.Base;
using Server.Core.Network.Helper;

namespace Server.Core.Network.TCP
{
    public class TCPChannel:AChannel
    {
        private Socket socket;
        private SocketAsyncEventArgs sendArgs = new SocketAsyncEventArgs();
        private SocketAsyncEventArgs recvArgs = new SocketAsyncEventArgs();
        
        private readonly CircularBuffer sendBuffer = new CircularBuffer();
        
        private bool isSending;

        private bool isRecving;

        private bool isConnected;

        private readonly PacketParser parser;

        private readonly byte[] packetSizeCache;
        
        public TCPChannel(AService service,IPEndPoint ipEndPoint ) : base(service, ChannelType.Connect)
        {
            int packetSize = service.PacketSizeLength;
            packetSizeCache = new byte[packetSize];
            this.memoryStream = service.MemoryStreamManager.GetStream("message", ushort.MaxValue);
			
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.NoDelay = true;
            parser = new PacketParser(packetSize, this.recvBuffer, this.memoryStream);
            sendArgs.Completed += this.OnComplete;
            recvArgs.Completed += this.OnComplete;

            RemoteAddress = ipEndPoint;

            isConnected = false;
            isSending = false;
        }
        
        public TCPChannel(AService service, Socket socket ) : base(service, ChannelType.Accept)
        {
            int packetSize = service.PacketSizeLength;
            this.packetSizeCache = new byte[packetSize];
            this.memoryStream = service.MemoryStreamManager.GetStream("message", ushort.MaxValue);
			
            this.socket = socket;
            this.socket.NoDelay = true;
            this.parser = new PacketParser(packetSize, this.recvBuffer, this.memoryStream);
            this.innArgs.Completed += this.OnComplete;
            this.outArgs.Completed += this.OnComplete;

            this.RemoteAddress = (IPEndPoint)socket.RemoteEndPoint;
			
            this.isConnected = true;
            this.isSending = false;
        }

        private TCPService GetService()
        {
            return (TCPService)Service;
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



        private void OnConnectComplete(object state)
        {
            if (socket == null)
            {
                return;
            }
            SocketAsyncEventArgs e = (SocketAsyncEventArgs) state;
			
            if (e.SocketError != SocketError.Success)
            {
                OnError((int)e.SocketError);	
                return;
            }

            e.RemoteEndPoint = null;
            isConnected = true;
			
            Start();
        }

        private void OnRecvComplete(object state)
        {
            if (this.socket == null)
            {
                return;
            }
            SocketAsyncEventArgs e = (SocketAsyncEventArgs) state;

            if (e.SocketError != SocketError.Success)
            {
                this.OnError((int)e.SocketError);
                return;
            }

            if (e.BytesTransferred == 0)
            {
                this.OnError(ErrorCode.ERR_PeerDisconnect);
                return;
            }

            this.recvBuffer.LastIndex += e.BytesTransferred;
            if (this.recvBuffer.LastIndex == this.recvBuffer.ChunkSize)
            {
                this.recvBuffer.AddLast();
                this.recvBuffer.LastIndex = 0;
            }

            // 收到消息回调
            while (true)
            {
                try
                {
                    if (!this.parser.Parse())
                    {
                        break;
                    }
                }
                catch (Exception ee)
                {
                    Log.Error(ee);
                    this.OnError(ErrorCode.ERR_SocketError);
                    return;
                }

                try
                {
                    this.OnRead(this.parser.GetPacket());
                }
                catch (Exception ee)
                {
                    Log.Error(ee);
                }
            }

            if (this.socket == null)
            {
                return;
            }
			
            this.StartRecv();
        }
        
        
        public override void Start()
        {
            if (!this.isConnected)
            {
                this.ConnectAsync(this.RemoteAddress);
                return;
            }

            if (!this.isRecving)
            {
                this.isRecving = true;
                this.StartRecv();
            }

            this.GetService().MarkNeedStartSend(this.Id);
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

            this.sendBuffer.Write(packetSizeCache, 0, packetSizeCache.Length);
            this.sendBuffer.Write(stream);

            GetService().MarkNeedStartSend(Id);
        }
    }
}