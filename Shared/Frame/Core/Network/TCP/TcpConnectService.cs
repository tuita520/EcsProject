using System;
using System.Net;

namespace Frame.Core.Network.TCP
{
    public class TcpConnectService : ATcpService
    {
        //Client
        public TcpConnectService(IPEndPoint ipEndPoint, Action<AChannel> callback) : base(callback)
        {
            _eventArgs.RemoteEndPoint = ipEndPoint;
            ConnectAsync();
        }
    }
}