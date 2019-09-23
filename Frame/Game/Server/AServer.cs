using Frame.Core.Base;
using Frame.Core.Server;
using Server.Core.Network.Helper;

namespace Frame.Core.Register
{
    public abstract class AServer:AEntity
    {
        public ServerType ServerType { get; private set; }
        public int AreaId { get; protected set; }
        public int SubId { get; protected set; }
        public string Key { get;protected set; }

        public AServer(ServerType serverType,int areaId,int subId)
        {
            ServerType = serverType;
            AreaId = areaId;
            SubId = subId;
            Key=ComponentKeyHelper.GetServerKey(serverType, areaId, subId);
        }
        
    }
}