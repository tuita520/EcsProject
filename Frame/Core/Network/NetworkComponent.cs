using System;
using System.Collections.Generic;
using Frame.Core.Base;
using Server.Core.Network.Helper;
using Server.Core.Network.TCP;

namespace Server.Core.Network
{
    public enum NetworkProtocol
    {
        TCP
    }

  

    
    public class NetworkComponent : AComponent
    {
        private AService Service;
        private readonly Dictionary<long, Session> sessions = new Dictionary<long, Session>();

//        public IMessagePacker MessagePacker { get; set; }

        
        public void Awake(object protocol)
        {
            Service = new TcpConnectService(OnConnect) {Parent = this};
        }
        
        public void Awake(object protocol,string address)
        {
            var ipEndPoint = NetworkHelper.ToIPEndPoint(address);
            Service = new TcpListenService(ipEndPoint, OnAccept) {Parent = this};
        }
        
        public void OnAccept(AChannel channel)
        {
            Session session = ComponentFactory.Create<Session, AChannel>(this, channel);
            sessions.Add(session.Id, session);
//            session.Start();
        }
        
        public void OnConnect(AChannel channel)
        {
            Session session = ComponentFactory.Create<Session, AChannel>(this, channel);
            sessions.Add(session.Id, session);
//            session.Start();
        }

    }
    
    public class NetworkComponentAwakeSystem : AAwakeSystem<NetworkComponent, string>
    {
        public override void Awake(NetworkComponent self, string address)
        {
            self.Awake(NetworkProtocol.TCP, address);
//            self.MessagePacker = new ProtobufPacker();
//            self.MessageDispatcher = new OuterMessageDispatcher();
        }
    }
}