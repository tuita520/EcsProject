using System;

namespace Frame.Core.Base
{
    public interface ILateUpdateSystem
    {
        Type Type();
        void Run(object o);
    }

    
    public abstract class ALateUpdateSystem<T>:ILateUpdateSystem
    {
        public Type Type()
        {
            return typeof(T);
        }

        public void Run(object o)
        {
            this.LateUpdate((T)o);
        }
        
        public abstract void LateUpdate(T self);
    }
}