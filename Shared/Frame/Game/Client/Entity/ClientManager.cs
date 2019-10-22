using Frame.Core.Base;
using Frame.Core.Base.Attributes;
using Frame.Core.Network;

namespace Game.Module
{
    public class ClientManager:ARegisterCenter
    {
        public override void Awake()
        {
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

}