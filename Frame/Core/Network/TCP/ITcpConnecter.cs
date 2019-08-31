using System.Net;

namespace Server.Core.Network.TCP
{
    public interface ITcpConnecter
    {
        void Connect(IPEndPoint ipEndPoint);
    }
}