﻿using System.Net;
using Frame.Core.Base;
using Frame.Core.Network;
using Module;

namespace Game.Module
{
    public class Client: AEntity
    {
        public string Token;
        public string PlayerUid;
        public IPEndPoint IpEndPoint;
        public Player Player;
    }
}