using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Frame.Core.Base;
using Microsoft.IO;
using RDLog;

namespace Server.Core.Network.TCP
{
    public class TcpListenService:AService
    {
        private readonly Dictionary<long, TcpChannel> _idChannels = new Dictionary<long, TcpChannel>();
        
        private readonly Socket _socket;
 
        private readonly SocketAsyncEventArgs _eventArgs = new SocketAsyncEventArgs();
        
        private readonly HashSet<long> _needStartSendChannel = new HashSet<long>();
        
        //Server
        public TcpListenService(IPEndPoint ipEndPoint, Action<AChannel> callback):base(callback)
        {
            PacketSizeLength = Packet.PacketSizeLength2;
            
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _eventArgs.Completed += OnComplete;
            
            _socket.Bind(ipEndPoint);
            _socket.Listen(1000);

            AcceptAsync();
        }

        private void AcceptAsync()
        {
            _eventArgs.AcceptSocket = null;
            if (_socket.AcceptAsync(_eventArgs))
            {
                return;
            }
            OnAcceptComplete(_eventArgs);
        }

        public override AChannel GetChannel(long id)
        {
            _idChannels.TryGetValue(id, out var channel);
            return channel;
        }

        public override void Remove(long channelId)
        {
            if (!_idChannels.TryGetValue(channelId, out var channel))
            {
                return;
            }
            if (channel == null)
            {
                return;
            }
            _idChannels.Remove(channelId);
            channel.Dispose();
        }

        public override void Update()
        {
            foreach (var id in _needStartSendChannel)
            {
                if (!_idChannels.TryGetValue(id, out var channel))
                {
                    continue;
                }

                if (channel.IsSending)
                {
                    continue;
                }

                try
                {
                    channel.StartSend();
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
			
            _needStartSendChannel.Clear();
        }
        
        private void OnComplete(object sender, SocketAsyncEventArgs eventArgs)
        {
            switch (eventArgs.LastOperation)
            {
                case SocketAsyncOperation.Accept:
                    MainThreadSynchronizationContext.Inst.Post(OnAcceptComplete, eventArgs);
                    break;
                default:
                    throw new Exception($"socket accept error: {eventArgs.LastOperation}");
            }
        }

        private void OnAcceptComplete(object o)
        {
            if (_socket == null)
            {
                return;
            }
            var e = (SocketAsyncEventArgs)o;
			
            if (e.SocketError != SocketError.Success)
            {
                Log.Error($"accept error {e.SocketError}");
                AcceptAsync();
                return;
            }
            var channel = new TcpChannel(this,e.AcceptSocket);
            _idChannels[channel.Id] = channel;

            try
            {
                OnAction(channel);
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

            if (_socket == null)
            {
                return;
            }
			
            AcceptAsync();
        }
        
        public void MarkNeedStartSend(long id)
        {
            _needStartSendChannel.Add(id);
        }
    }
}