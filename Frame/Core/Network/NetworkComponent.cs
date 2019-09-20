using System;
using System.Collections.Generic;
using System.Linq;
using Frame.Core.Base;
using Frame.Core.Base.Attributes;
using Frame.Core.Message;
using Server.Core.Network.Helper;
using Server.Core.Network.TCP;

namespace Server.Core.Network
{
    public class NetworkComponent : AComponent
    {
        protected AService service;
        private readonly Dictionary<long, Session> sessions = new Dictionary<long, Session>();

        public IMessagePacker MessagePacker { get; set; }
    
        public void AddSession(Session session)
        {
            sessions.Add(session.Id,session);
        }

        public void RemoveSession(long id)
        {
            if (!sessions.TryGetValue(id, out var session))
            {
                return;
            }
            sessions.Remove(id);
            session.Dispose();
        }

        public Session Get(long id)
        {
            sessions.TryGetValue(id, out var session);
            return session;
        }
        
        public void Update()
        {
            service?.Update();
        }
        
        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            base.Dispose();

            foreach (var item in sessions)
            {
                item.Value.Dispose();
            }

            service.Dispose();
        }
        
    }
    
}