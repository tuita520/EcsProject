using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Frame.Core.Base;
using RDLog;

namespace Frame.Core.Network.TCP
{
    public class TcpService:AService
    {
        private NetworkType _type;
        
        private readonly Dictionary<long, TcpChannel> _idChannels;
        
        protected Socket _socket;
 
        protected readonly SocketAsyncEventArgs _eventArgs ;
        
        private readonly HashSet<long> _needStartSendChannel;
        
        public TcpService(NetworkType type, IPEndPoint ipEndPoint,Action<AChannel> callback) : base(callback)
        {
            PacketSizeLength = Packet.PacketSizeLength2;
            
            _idChannels= new Dictionary<long, TcpChannel>();
            _needStartSendChannel = new HashSet<long>();

            _eventArgs = new SocketAsyncEventArgs();
            _eventArgs.Completed += OnComplete;
            
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _type  = type;
            switch (type)
            {
                case NetworkType.Connector:
                    _eventArgs.RemoteEndPoint = ipEndPoint;
                    ConnectAsync();
                    break;
                case NetworkType.Listener:
                    _socket.Bind(ipEndPoint);
                    _socket.Listen(1000);
                    AcceptAsync();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        
        protected void AcceptAsync()
        {
            Log.Debug($"accept on {_socket.LocalEndPoint}");
            _eventArgs.AcceptSocket = null;
            if (_socket.AcceptAsync(_eventArgs))
            {
                return;
            }

            OnAcceptComplete(_eventArgs);
        }
        
        public void ConnectAsync()
        {
            Log.Debug($"try to connect to {_eventArgs.RemoteEndPoint}");
            if (_socket.ConnectAsync(_eventArgs))
            {
                return;
            }

            OnConnectComplete(_eventArgs);
        }

        public void ReconnectAsync()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            ConnectAsync();
        }
        
        private void OnAcceptComplete(object o)
        {
            if (_socket == null)
            {
                return;
            }

            var e = (SocketAsyncEventArgs) o;

            if (e.SocketError != SocketError.Success)
            {
                Log.Error($"accept error {e.SocketError}");
                AcceptAsync();
                return;
            }

            var channel = new TcpChannel(this, e.AcceptSocket,ChannelType.Accept);
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
        
        private void OnConnectComplete(object o)
        {
            if (_socket == null)
            {
                return;
            }

            var e = (SocketAsyncEventArgs) o;

            if (e.SocketError != SocketError.Success)
            {
                //重连
                Log.Error($"connect error : {e.SocketError},try to reconnect to {e.RemoteEndPoint}");
                ConnectAsync();               
                return;
            }

            var channel = new TcpChannel(this, e.ConnectSocket,ChannelType.Connect);
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

        private void OnComplete(object sender, SocketAsyncEventArgs eventArgs)
        {
            switch (eventArgs.LastOperation)
            {
                case SocketAsyncOperation.Connect:
                    MainThreadSynchronizationContext.Inst.Post(OnConnectComplete, eventArgs);
                    break;
                case SocketAsyncOperation.Accept:
                    MainThreadSynchronizationContext.Inst.Post(OnAcceptComplete, eventArgs);
                    break;
                default:
                    throw new Exception($"socket connect error: {eventArgs.LastOperation}");
            }
        }

        public override void Dispose()
        {
            Log.Debug("TcpService dispose");

            if (IsDisposed)
            {
                return;
            }
			
            base.Dispose();

            foreach (var id in _idChannels.Keys.ToArray())
            {
                var channel = _idChannels[id];
                channel.Dispose();
            }
            _socket?.Close();
            _socket = null;
            _eventArgs.Dispose();
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

        
        public void MarkNeedStartSend(long id)
        {
            _needStartSendChannel.Add(id);
        }
        
                
//        private void DisconnectAsync()
//        {
//            if (_socket.DisconnectAsync(_eventArgs))
//            {
//                return;
//            }
//            OnDisconnectComplete(_eventArgs);
//        }
//
//        private void OnDisconnectComplete(SocketAsyncEventArgs eventArgs)
//        {
//            throw new NotImplementedException();
//        }
    }
}