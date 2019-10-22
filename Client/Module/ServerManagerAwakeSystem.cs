using Frame.Core.Base;
using Frame.Core.Base.Attributes;
using Frame.Core.Network;
using Game.Module.Server;

namespace Client.Module
{
    [System]
    public class ServerManagerAwakeSystem : AAwakeSystem<ServerManager>
    {
        protected override void Awake(ServerManager self)
        {
            self.Awake();
            self.AddComponent<NetworkComponent,NetworkProtocol,NetworkType,string,bool>(NetworkProtocol.TCP ,NetworkType.Connector,"127.0.0.1:50000",true);
        }
    }
 
}