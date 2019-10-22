using Frame.Core.Base;
using Frame.Core.Base.Attributes;
using Frame.Core.Network;
using Game.Module;

namespace Server.Module
{
    
    [System]
    public class ClientManagerAwakeSystem : AAwakeSystem<ClientManager>
    {
        protected override void Awake(ClientManager self)
        {
            self.Awake();
            self.AddComponent<NetworkComponent,NetworkProtocol,NetworkType,string,bool>(NetworkProtocol.TCP ,NetworkType.Listener,"127.0.0.1:50000",false);
        }
    }
}