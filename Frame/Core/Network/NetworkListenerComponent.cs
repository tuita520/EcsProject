using System;
using System.Collections.Generic;
using Frame.Core.Base;
using Frame.Core.Base.Attributes;
using Frame.Core.Message;
using Frame.Core.Register;
using RDLog;
using Server.Core.Network.Helper;
using Server.Core.Network.TCP;

namespace Server.Core.Network
{
    public class NetworkListenerComponent : NetworkComponent
    {
        public void Awake(object protocol,string address)
        {
            var ipEndPoint = NetworkHelper.ToIPEndPoint(address);
            service = new TcpListenService(ipEndPoint, OnAccept) {Parent = this};
        }

        private void OnAccept(AChannel channel)
        {
            var session = ComponentFactory.Create<Session, AChannel>(this, channel);
            AddSession(session);
            session.Start();
        }
        
    }
    
    [System]
    public class NetListenerComponentAwakeSystem : AAwakeSystem<NetworkListenerComponent,NetworkProtocol, string>
    {
        protected override void Awake(NetworkListenerComponent self, NetworkProtocol protocol,string address)
        {
            self.Awake(protocol, address);
            self.MessagePacker = new StringPacker();
//            self.MessageDispatcher = new OuterMessageDispatcher();
        }
    }
}