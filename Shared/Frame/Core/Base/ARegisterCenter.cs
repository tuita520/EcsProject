using System.Collections.Generic;
using Frame.Core.Base.Attributes;
using Frame.Core.Network;
using Frame.Core.Register;

namespace Frame.Core.Base
{
    public abstract class ARegisterCenter : AEntity
    {
        private bool needRegister { get; set; }

        private Dictionary<string, AEntity> _entitys = new Dictionary<string, AEntity>();

        public ARegisterCenter()
        {
            needRegister = false;
        }
        
        public void Awake(bool needRegister = false)
        {
            this.needRegister = needRegister;
        }

        public void Add(AServer entity)
        {
            _entitys.Add(entity.Key, entity);
        }

        public AEntity Get(string key)
        {
            _entitys.TryGetValue(key, out var entity);
            return entity;
        }

        public void Remove(string key)
        {
            _entitys.Remove(key);
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            base.Dispose();

            foreach (var actor in _entitys.Values)
            {
                actor.Dispose();
            }

        }

        public void SendRegisterMsg(Session session)
        {
            if (needRegister)
            {
                App.Inst.Zone.SendActorRegisterMsg(session);
            }
        }
    }

    [System]
    public class RegisterCenterAwakeSystem : AAwakeSystem<ARegisterCenter, bool>
    {
        protected override void Awake(ARegisterCenter self, bool needRegister)
        {
            self.Awake(needRegister);
        }
    }
}