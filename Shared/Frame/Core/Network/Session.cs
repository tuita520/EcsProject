using System;
using System.Collections.Generic;
using System.IO;
using Frame.Core.Base;
using Frame.Core.Base.Attributes;
using Frame.Core.Message;
using Frame.Core.Network.TCP;
using RDLog;

namespace Frame.Core.Network
{
    public sealed class Session : AEntity
    {
        public AChannel Channel { get;  private set;}

        private NetworkComponent network => GetParent<NetworkComponent>();

        private readonly Dictionary<int, Action<IResponse>> requestCallback = new Dictionary<int, Action<IResponse>>();

        public void Awake(AChannel aChannel)
        {
            Channel = aChannel;
            requestCallback.Clear();
            var id = Id;
            Channel.ErrorCallback += (c, e) => { network.RemoveSession(id); };
            Channel.ReadCallback += OnRead;
        }

        private void OnRead(MemoryStream memoryStream)
        {
            try
            {
                Run(memoryStream);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        private void Run(MemoryStream memoryStream)
        {
            memoryStream.Seek(0, SeekOrigin.Begin);
            ushort opcode = BitConverter.ToUInt16(memoryStream.GetBuffer(), Packet.OpcodeIndex);

            object message;
            try
            {
//                message = network.MessagePacker.DeserializeFrom(opcode, memoryStream);

//                if (OpcodeHelper.IsNeedDebugLogMessage(opcode))
//                {
//                    Log.Msg(message);
//                }
            }
            catch (Exception e)
            {
                // 出现任何消息解析异常都要断开Session，防止客户端伪造消息
                Log.Error($"opcode: {opcode} {e} ");
                Channel.Error = NetworkErrorCode.PacketParserError;
                network.RemoveSession(Id);
                return;
            }

//            IResponse response = message as IResponse;
//            if (response == null)
//            {
//                this.network.MessageDispatcher.Dispatch(this, opcode, message);
//                return;
//            }
//			
//            Action<IResponse> action;
//            if (!this.requestCallback.TryGetValue(response.RpcId, out action))
//            {
//                throw new Exception($"not found rpc, response message: {StringHelper.MessageToStr(response)}");
//            }
//            this.requestCallback.Remove(response.RpcId);
//
//            action(response);
        }
        
        public void Send(MemoryStream stream)
        {
            Channel.Send(stream);
        }

        public void Start()
        {
            Channel.Start();
        }
    }

    [System]
    public class SessionAwakeSystem : AAwakeSystem<Session, AChannel>
    {
        protected override void Awake(Session self, AChannel b)
        {
            self.Awake(b);
        }
    }

    [System]
    public class SessionStartSystem : AStartSystem<Session>
    {
        public override void Start(Session self)
        {
            self.Start();
        }
    }
}