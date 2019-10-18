﻿using System;
using System.Net;
using Frame.Core.Base;
using Microsoft.IO;

namespace Frame.Core.Network
{
    public abstract class AService:AComponent
    {
        public readonly RecyclableMemoryStreamManager MemoryStreamManager;

        private Action<AChannel> callback;
        
        public AService(Action<AChannel> callback)
        {
            MemoryStreamManager = new RecyclableMemoryStreamManager();
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