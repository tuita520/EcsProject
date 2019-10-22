using System.Collections.Generic;
using Frame.Core.Base.Attributes;
using Frame.Core.Network;
using Frame.Core.Register;

namespace Frame.Core.Base
{
    /// <summary>
    /// 注册中心，已经通过验证的session会存储在这里
    /// 这是个抽象类，需要具体实现，重写 OnConnect 和 OnDisConnect ，来实现连接和断开后的不同注册逻辑。
    /// </summary>
    public abstract class ARegisterCenter : AEntity
    {
        private Dictionary<long, AEntity> _entitys = new Dictionary<long, AEntity>();
        public void Add(AEntity entity)
        {
            _entitys.Add(entity.Key, entity);
        }

        public AEntity Get(long key)
        {
            _entitys.TryGetValue(key, out var entity);
            return entity;
        }

        public void Remove(long key)
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

        public abstract void OnDisConnect(Session session);

        public abstract void OnConnect(Session session);

        public abstract void Awake();
    }

//    [System]
//    public class RegisterCenterAwakeSystem : AAwakeSystem<ARegisterCenter>
//    {
//        protected override void Awake(ARegisterCenter self)
//        {
//            self.Awake();
//        }
//    }

}