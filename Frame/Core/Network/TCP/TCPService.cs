using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Frame.Core.Base;
using Microsoft.IO;
using RDLog;

namespace Server.Core.Network.TCP
{
    public class TCPService:AService
    {
        private readonly Dictionary<long, TCPChannel> _idChannels = new Dictionary<long, TCPChannel>();

        public int PacketSizeLength { get; }
        
        private readonly Socket _sockt;
 
        private readonly SocketAsyncEventArgs _eventArgs = new SocketAsyncEventArgs();
        
        private readonly HashSet<long> _needStartSendChannel = new HashSet<long>();
        
        public RecyclableMemoryStreamManager MemoryStreamManager = new RecyclableMemoryStreamManager();

        //Server
        public TCPService(int packetSizeLength, Action<AChannel> callback):base(callback)
        {
            PacketSizeLength = packetSizeLength;
            _sockt = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _sockt.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            
            _eventArgs.Completed += OnComplete;
        }

        public void Bind(IPEndPoint ipEndPoint)
        {
            _sockt.Bind(ipEndPoint);
            _sockt.Listen(1000);
            AcceptAsync();
        }

        public void Connect(IPEndPoint ipEndPoint)
        {
            _eventArgs.RemoteEndPoint = ipEndPoint;
            ConnectAsync();
        }
        
        private void AcceptAsync()
        {
            _eventArgs.AcceptSocket = null;
            if (_sockt.AcceptAsync(_eventArgs))
            {
                return;
            }
            OnAcceptComplete(_eventArgs);
        }

        private void ConnectAsync()
        {
            if (_sockt.ConnectAsync(_eventArgs))
            {
                return;
            }
            OnConnectComplete(_eventArgs);
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
                case SocketAsyncOperation.Connect:
                    MainThreadSynchronizationContext.Inst.Post(OnConnectComplete,eventArgs);
                    break;
                default:
                    throw new Exception($"socket accept error: {eventArgs.LastOperation}");
            }
        }

        private void OnAcceptComplete(object o)
        {
            if (_sockt == null)
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
            var channel = new TCPChannel(this,e.AcceptSocket);
            _idChannels[channel.Id] = channel;

            try
            {
                OnAction(channel);
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

            if (_sockt == null)
            {
                return;
            }
			
            AcceptAsync();
        }
        
        private void OnConnectComplete(object o)
        {
            if (_sockt == null)
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
            TCPChannel channel = new TCPChannel(this,e.ConnectSocket);
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

        public void MarkNeedStartSend(long id)
        {
            _needStartSendChannel.Add(id);
        }
    }
}