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
        
        public void Awake(NetworkProtocol protocol, NetworkType type, string address)
        {
            _protocol = protocol;
            _type = type;
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
            if (_type.Equals(NetworkType.Connector))
            {
                (Parent as ARegisterCenter)?.SendRegisterMsg(session);
            }
        }

        public void OnDisConnect(Session session)
        {
            Log.Info($"disconnect !(local:{session.Channel.LocalAddress}  remote:{session.Channel.RemoteAddress})");
            RemoveSession(session.Id);
            if (_type == NetworkType.Connector)
            {
                //TODO:BOIL 重连
                if (_protocol==NetworkProtocol.TCP)
                {
                    (_service as TcpService)?.ReconnectAsync();
                }
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
    public class NetWorkComponentAwakeSystem : AAwakeSystem<NetworkComponent, NetworkProtocol, NetworkType, string>
    {
        protected override void Awake(NetworkComponent self, NetworkProtocol protocol, NetworkType type, string address)
        {
            self.Awake(protocol, type, address);
            self.MessagePacker = new StringPacker();
//            self.MessageDispatcher = new OuterMessageDispatcher();
        }
    }
}