using Frame.Core.Base;
using Frame.Core.Base.Attributes;
using Frame.Core.Message;
using Frame.Core.Network.Helper;
using Frame.Core.Network.TCP;

namespace Frame.Core.Network
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
         
            (Parent as ARegisterCenter)?.SendRegisterMsg(session);
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