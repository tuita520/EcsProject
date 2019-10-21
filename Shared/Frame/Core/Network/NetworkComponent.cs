using System;
using System.Collections.Generic;
using Frame.Core.Base;
using Frame.Core.Base.Attributes;
using Frame.Core.Message;
using Frame.Core.Network.Helper;
using Frame.Core.Network.TCP;
using RDLog;

namespace Frame.Core.Network
{
    public class NetworkComponent : AComponent
    {
        protected AService _service;
        private readonly Dictionary<long, Session> sessions = new Dictionary<long, Session>();
        public IMessagePacker MessagePacker { get; set; }

        private NetworkProtocol _protocol;
        private NetworkType _type;

        /// <summary>
        /// 断线后操作需要重连
        /// </summary>
        private bool _needReconnect { get; set; }

        public void Awake(NetworkProtocol protocol, NetworkType type, string address,bool needReconnect)
        {
            _protocol = protocol;
            _type = type;
            _needReconnect = needReconnect;
            
            var ipEndPoint = NetworkHelper.ToIPEndPoint(address);
            switch (protocol)
            {
                case NetworkProtocol.TCP:
                    _service = new TcpService(type, ipEndPoint, OnConnect) {Parent = this};
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(protocol), protocol, null);
            }
        }
        
        private void OnConnect(AChannel channel)
        {
            var session = ComponentFactory.Create<Session, AChannel>(this, channel);
            AddSession(session);
            session.Start();
            
            Log.Info($"connection success! (local:{session.Channel.LocalAddress}  remote:{session.Channel.RemoteAddress}"); 
            (Parent as ARegisterCenter)?.OnConnect(session);
        }

        public void OnDisConnect(Session session)
        {
            var sessionId = session.Id;
            RemoveSession(sessionId);
       
            Log.Info($"disconnect !(local:{session.Channel.LocalAddress}  remote:{session.Channel.RemoteAddress})");

            (Parent as ARegisterCenter)?.OnDisConnect(session);
            
            if (_needReconnect)
            {
                (_service as TcpService)?.ReconnectAsync();
            }
        }
        
        public void AddSession(Session session)
        {
            sessions.Add(session.Id, session);
        }

        public void RemoveSession(long id)
        {
            if (!sessions.TryGetValue(id, out var session))
            {
                return;
            }

            sessions.Remove(id);
            session.Dispose();
        }

        public Session Get(long id)
        {
            sessions.TryGetValue(id, out var session);
            return session;
        }

        public void Update()
        {
            _service?.Update();
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            base.Dispose();
            foreach (var item in sessions)
            {
                item.Value.Dispose();
            }

            _service.Dispose();
        }
    }

    [System]
    public class NetWorkComponentAwakeSystem : AAwakeSystem<NetworkComponent, NetworkProtocol, NetworkType, string,bool>
    {
        protected override void Awake(NetworkComponent self, NetworkProtocol protocol, NetworkType type, string address,bool needReconnect)
        {
            self.Awake(protocol, type, address,needReconnect);
            self.MessagePacker = new StringPacker();
//            self.MessageDispatcher = new OuterMessageDispatcher();
        }
    }
    
}