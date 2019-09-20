using System;
using System.IO;
using System.Net;
using Frame.Core.Base;
using Server.Core.Network.TCP;

namespace Server.Core.Network
{
    public enum ChannelType
    {
        Connect,
        Accept
    }
    
    public abstract class AChannel:AComponent
    {
        public ChannelType ChannelType { get; }

        public AService Service { get; private set; }
        
        public IPEndPoint RemoteAddress { get; protected set; }
        
        private Action<MemoryStream> readCallback;

        public event Action<MemoryStream> ReadCallback
        {
            add => readCallback += value;
            remove => readCallback -= value;
        }

        public int Error { get; set; }


        private Action<AChannel, int> errorCallback;

        public event Action<AChannel, int> ErrorCallback
        {
            add => errorCallback += value;
            remove => errorCallback -= value;
        }

        protected AChannel(AService service, ChannelType channelType)
        {
            ChannelType = channelType;
            Service = service;
        }
        
        protected void OnRead(MemoryStream memoryStream)
        {
            readCallback.Invoke(memoryStream);
        }
        
        protected void OnError(int e)
        {
            Error = e;
            errorCallback?.Invoke(this, e);
        }

        public abstract void Send(MemoryStream stream);

        public abstract void Start();
    }
}