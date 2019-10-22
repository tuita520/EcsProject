using Frame.Core.Base;
using Frame.Core.Base.Attributes;
using Frame.Core.Network;

namespace Game.Module
{
    public class ClientManager:ARegisterCenter
    {
        public override void Awake()
        {
            AddComponent<NetworkComponent,NetworkProtocol,NetworkType,string,bool>(NetworkProtocol.TCP ,NetworkType.Listener,"127.0.0.1:50000",false);
        }

        public override void OnDisConnect(Session session)
        {
            throw new System.NotImplementedException();
        }

        public override void OnConnect(Session session)
        {
            throw new System.NotImplementedException();
        }
   
    }
    
    [System]
    public class ClientManagerAwakeSystem : AAwakeSystem<ClientManager>
    {
        protected override void Awake(ClientManager self)
        {
            self.Awake();
        }
    }
}