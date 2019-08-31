using System;
using System.Net;
using Frame.Core.Base;
using Microsoft.IO;

namespace Server.Core.Network
{
    public abstract class AService:AComponent
    {
        public readonly RecyclableMemoryStreamManager MemoryStreamManager = new RecyclableMemoryStreamManager();
        
        private Action<AChannel> callback;
        
        public AService(Action<AChannel> callback)
        {
            this.callback += callback;
        }
        
        public abstract AChannel GetChannel(long id);

        protected void OnAction(AChannel channel)
        {
            callback.Invoke(channel);
        } 

        public abstract void Remove(long channelId);

        public abstract void Update();
        
        public int PacketSizeLength { get; set; }
    }
}