using System;
using System.Net;

namespace Server.Core.Network.TCP
{
    public class TcpListenService : ATcpService
    {
        //Server
        public TcpListenService(IPEndPoint ipEndPoint, Action<AChannel> callback) : base(callback)
        {
            _socket.Bind(ipEndPoint);
            _socket.Listen(1000);
            AcceptAsync();
        }
    }
}