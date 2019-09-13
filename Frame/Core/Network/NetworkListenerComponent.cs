using System;
using System.Collections.Generic;
using Frame.Core.Base;
using Frame.Core.Base.Attributes;
using Frame.Core.Message;
using Server.Core.Network.Helper;
using Server.Core.Network.TCP;

namespace Server.Core.Network
{
    public class NetworkListenerComponent : NetworkComponent
    {
        public void Awake(object protocol,string address)
        {
            var ipEndPoint = NetworkHelper.ToIPEndPoint(address);
            Service = new TcpListenService(ipEndPoint, OnAccept) {Parent = this};
        }

        private void OnAccept(AChannel channel)
        {
            Session session = ComponentFactory.Create<Session, AChannel>(this, channel);
            AddSession(session);
            session.Start();
        }
    }
    
    [System]
    public class NetListenerComponentAwakeSystem : AAwakeSystem<NetworkListenerComponent, string>
    {
        protected override void Awake(NetworkListenerComponent self, string address)
        {
            self.Awake(NetworkProtocol.TCP, address);
            self.MessagePacker = new StringPacker();
//            self.MessageDispatcher = new OuterMessageDispatcher();
        }
    }
}