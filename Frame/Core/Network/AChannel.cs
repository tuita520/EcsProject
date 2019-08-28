using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Frame.Core.Base;
using RDHelper;

namespace Server.Core.Network
{
    public enum ChannelType
    {
        Connect,
        Accept,
    }
    
    public abstract class AChannel:AComponent
    {
        public ChannelType ChannelType { get; }


        public AService Service { get; private set; }

        
        public IPEndPoint RemoteAddress { get; protected set; }
        

        private Action<MemoryStream> readCallback;

        public event Action<MemoryStream> ReadCallback
        {
            add
            {
                this.readCallback += value;
            }
            remove
            {
                this.readCallback -= value;
            }
        }

        public int Error { get; set; }


        private Action<AChannel, int> errorCallback;

        public event Action<AChannel, int> ErrorCallback
        {
            add
            {
                this.errorCallback += value;
            }
            remove
            {
                this.errorCallback -= value;
            }
        }

        
        protected AChannel(AService service, ChannelType channelType)
        {
            this.ChannelType = channelType;
        }
        
        public abstract void Start();
        
        protected void OnRead(MemoryStream memoryStream)
        {
            this.readCallback.Invoke(memoryStream);
        }
        
        protected void OnError(int e)
        {
            this.Error = e;
            this.errorCallback?.Invoke(this, e);
        }

        public abstract void Send(MemoryStream stream);

    }
}