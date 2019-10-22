using Frame.Core.Base;
using Frame.Core.Base.Attributes;
using Frame.Core.Network;

namespace Game.Module.Server
{
    public class ServerManager:ARegisterCenter
    {
        public override void Awake()
        {
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
    
    
//    [System]
//    public class ServerManagerAwakeSystem : AAwakeSystem<ServerManager>
//    {
//        protected override void Awake(ServerManager self)
//        {
//            self.Awake();
//        }
//    }
// 
}