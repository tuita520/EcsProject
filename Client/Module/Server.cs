using Frame.Core.Base;
using Frame.Core.Network;
using Frame.Core.Register;
using Frame.Core.Server;

namespace Client.Module
{
    public class Server:AServer
    {
        public Server(ServerType serverType, int areaId, int subId) : base(serverType, areaId, subId)
        {
        }
    }
}