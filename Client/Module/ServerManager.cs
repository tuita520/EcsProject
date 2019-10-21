using Frame.Core.Base;
using Frame.Core.Base.Attributes;
using Frame.Core.Network;

namespace Client.Module
{
    public class ServerManager:ARegisterCenter
    {
        private Server _server;
        public override void Awake()
        {
            AddComponent<NetworkComponent,NetworkProtocol,NetworkType,string,bool>(NetworkProtocol.TCP ,NetworkType.Connector,"127.0.0.1:50000",true);
        }
        
        public void Connect2Realm()
        {
            //TODO:BOILING 连接账号服
        }

        public void Connect2Gate()
        {
            //TODO:BOILING 连接gate服
        }
        
        public override void OnDisConnect(Session session)
        {
            //TODO:BOILING 断线
            //1 断线重连
            //2 重新登录
        }

        public override void OnConnect(Session session)
        {
             //TODO:BOILING 登录逻辑   
        }
        
        
    }
    
    
    [System]
    public class ServerManagerAwakeSystem : AAwakeSystem<ServerManager>
    {
        protected override void Awake(ServerManager self)
        {
            self.Awake();
        }
    }
 
}