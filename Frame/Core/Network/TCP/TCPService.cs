using System;
using System.Net;
using System.Net.Sockets;
using RDLog;

namespace Server.Core.Network.TCP
{
    public class TCPService:AService
    {
        public int PacketSizeLength { get; }
        
        private Socket socktAcceptor;
        private readonly SocketAsyncEventArgs sockeArgs = new SocketAsyncEventArgs();
        //Server
        public TCPService(int packetSizeLength, IPEndPoint ipEndPoint, Action<AChannel> acceptCallback)
        {
            PacketSizeLength = packetSizeLength;
            AcceptCallback += acceptCallback;
			
            socktAcceptor = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socktAcceptor.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            sockeArgs.Completed += OnAccepteComplete;
			
            socktAcceptor.Bind(ipEndPoint);
            socktAcceptor.Listen(1000);
            AcceptAsync();
        }

        //client
        public TCPService(int packetSizeLength)
        {
            PacketSizeLength = packetSizeLength;
        }
        
        
        public override AChannel GetChannel(long id)
        {
            throw new NotImplementedException();
        }

        public override AChannel Connect(IPEndPoint ipEndPoint)
        {
            throw new NotImplementedException();
        }

        public override AChannel Connect(string address)
        {
            throw new NotImplementedException();
        }

        public override void Remove(long channelId)
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
        
        
        private void OnAccepteComplete(object sender, SocketAsyncEventArgs e)
        {
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Accept:
                    OneThreadSynchronizationContext.Instance.Post(this.OnAcceptComplete, e);
                    break;
                default:
                    throw new Exception($"socket accept error: {e.LastOperation}");
            }
        }
        
        		
        private void AcceptAsync()
        {
            sockeArgs.AcceptSocket = null;
            if (acceptor.AcceptAsync(sockeArgs))
            {
                return;
            }
            OnAcceptComplete(sockeArgs);
        }

        private void OnAcceptComplete(object o)
        {
            if (acceptor == null)
            {
                return;
            }
            SocketAsyncEventArgs e = (SocketAsyncEventArgs)o;
			
            if (e.SocketError != SocketError.Success)
            {
                Log.Error($"accept error {e.SocketError}");
                AcceptAsync();
                return;
            }
            TCPChannel channel = new TCPChannel(this,e.AcceptSocket);
            this.idChannels[channel.Id] = channel;

            try
            {
                OnAccept(channel);
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

            if (acceptor == null)
            {
                return;
            }
			
            AcceptAsync();
        }

        
    }
}