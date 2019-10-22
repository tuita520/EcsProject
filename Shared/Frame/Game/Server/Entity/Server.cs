using EnumData;

namespace Game.Module.Server
{
    public class Server:AServer
    {
        public Server(ServerType serverType, int areaId, int subId) : base(serverType, areaId, subId)
        {
        }
    }
}