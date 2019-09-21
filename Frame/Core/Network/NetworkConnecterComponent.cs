using System;
using System.Collections.Generic;
using System.IO;
using Frame.Core.Base;
using Frame.Core.Base.Attributes;
using Frame.Core.Message;
using RDLog;
using Server.Core.Network.Helper;
using Server.Core.Network.TCP;

namespace Server.Core.Network
{
    public class NetworkConnecterComponent : NetworkComponent
    {
        public void Awake(object protocol,string address)
        {
            var ipEndPoint = NetworkHelper.ToIPEndPoint(address);
            service = new TcpConnectService(ipEndPoint, OnConnect) {Parent = this};
        }

        private void OnConnect(AChannel channel)
        {
            var session = ComponentFactory.Create<Session, AChannel>(this, channel);
            AddSession(session);
            session.Start();
        }
    }
    
    [System]
    public class NetConnecterComponentAwakeSystem : AAwakeSystem<NetworkConnecterComponent, NetworkProtocol,string>
    {
        protected override void Awake(NetworkConnecterComponent self, NetworkProtocol protocol,string address)
        {
            self.Awake(protocol, address);
            self.MessagePacker = new StringPacker();
//            self.MessageDispatcher = new OuterMessageDispatcher();
        }
    }


}