using System;
using System.Collections.Generic;
using System.Net;
using Frame.Core.Base;
using Server.Core.Network.Helper;
using Server.Core.Network.TCP;

namespace Server.Core.Network
{
    public enum NetworkProtocol
    {
        TCP,
    }

    public class NetworkComponent : AComponent
    {
        protected AService Service;
        private readonly Dictionary<long, Session> sessions = new Dictionary<long, Session>();

        public void Bind(NetworkProtocol protocol)
        {
            this.Service = new TCPService( Packet.PacketSizeLength2 ) {Parent = this};
        }

        public void Connect(NetworkProtocol protocol, string address)
        {
            try
            {
                var ipEndPoint = NetworkHelper.ToIPEndPoint(address);
                this.Service = new TCPService(Packet.PacketSizeLength2,ipEndPoint, this.OnAccept) {Parent = this};
            }
            catch (Exception e)
            {
                throw new Exception($"NetworkComponent Awake Error {address}", e);
            }
        }
        
        public void OnAccept(AChannel channel)
        {
            Session session = ComponentFactory.Create<Session, AChannel>(this, channel);
            this.sessions.Add(session.Id, session);
            session.Start();
        }
    }
}