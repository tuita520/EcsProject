using System.Net;

namespace Server.Core.Network.TCP
{
    public interface ITcpListener
    {
        void Bind(IPEndPoint ipEndPoint);
        void Listen();
    }
}