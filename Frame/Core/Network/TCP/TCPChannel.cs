using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Server.Core.Network.TCP
{
    public class TCPChannel:AChannel
    {
        private Socket socket;
        private SocketAsyncEventArgs accpeterArgs = new SocketAsyncEventArgs();
        
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
            this.innArgs.Completed += this.OnComplete;
            outArgs.Completed += this.OnComplete;

            RemoteAddress = ipEndPoint;

            isConnected = false;
            isSending = false;
        }
        
        public TCPChannel(AService service, Socket socket ) : base(service, ChannelType.Connect)
        {
            
        }

        private TCPService GetService()
        {
            return (TCPService)Service;
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