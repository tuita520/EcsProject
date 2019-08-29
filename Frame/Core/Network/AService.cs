using System;
using System.Net;
using Frame.Core.Base;

namespace Server.Core.Network
{
    public abstract class AService:AComponent
    {
        public abstract AChannel GetChannel(long id);

        private Action<AChannel> acceptCallback;

        public event Action<AChannel> AcceptCallback
        {
            add
            {
                acceptCallback += value;
            }
            remove
            {
                acceptCallback -= value;
            }
        }
		
        protected void OnAccept(AChannel channel)
        {
            acceptCallback.Invoke(channel);
        }

        public abstract AChannel Connect(IPEndPoint ipEndPoint);
		
        public abstract AChannel Connect(string address);

        public abstract void Remove(long channelId);

        public abstract void Update();
        public int PacketSizeLength { get; set; }
    }
}