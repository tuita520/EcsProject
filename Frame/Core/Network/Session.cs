using Frame.Core.Base;

namespace Server.Core.Network
{
    public sealed class Session:AEntity
    {
        private AChannel channel;
        
        public void Start()
        {
            channel.Start();
        }

    }
}