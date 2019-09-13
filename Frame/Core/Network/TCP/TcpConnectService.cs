using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Frame.Core.Base;
using Microsoft.IO;
using RDLog;

namespace Server.Core.Network.TCP
{
    public class TcpConnectService:AService
    {
        private readonly Dictionary<long, TcpChannel> _idChannels = new Dictionary<long, TcpChannel>();
        
        private readonly Socket _socket;
 
        private readonly SocketAsyncEventArgs _eventArgs = new SocketAsyncEventArgs();
        
        private readonly HashSet<long> _needStartSendChannel = new HashSet<long>();
        
        //Server
        public TcpConnectService(IPEndPoint ipEndPoint,Action<AChannel> callback):base(callback)
        {
            PacketSizeLength = Packet.PacketSizeLength2;
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _eventArgs.Completed += OnComplete;
            _eventArgs.RemoteEndPoint = ipEndPoint;
            ConnectAsync();
        }

        private void ConnectAsync()
        {
            if (_socket.ConnectAsync(_eventArgs))
            {
                return;
            }
            OnConnectComplete(_eventArgs);
        }
        
        public void MarkNeedStartSend(long id)
        {
            _needStartSendChannel.Add(id);
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
                case SocketAsyncOperation.Connect:
                    MainThreadSynchronizationContext.Inst.Post(OnConnectComplete,eventArgs);
                    break;
                default:
                    throw new Exception($"socket connect error: {eventArgs.LastOperation}");
            }
        }

        private void OnConnectComplete(object o)
        {
            if (_socket == null)
            {
                return;
            }
            SocketAsyncEventArgs e = (SocketAsyncEventArgs)o;
			
            if (e.SocketError != SocketError.Success)
            {
                Log.Error($"connect error {e.SocketError}");
                //重连
                ConnectAsync();
                return;
            }
            TcpChannel channel = new TcpChannel(this,e.ConnectSocket);
            _idChannels[channel.Id] = channel;

            try
            {
                OnAction(channel);
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

 
    }
}