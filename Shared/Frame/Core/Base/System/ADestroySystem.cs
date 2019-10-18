using System;

namespace Frame.Core.Base
{
    public interface IDestroySystem
    {
        Type Type();
        void Run(object o);
    }
    
    public abstract class ADestroySystem<T>:IDestroySystem
    {
        public Type Type()
        {
            return typeof(T);
        }

        public void Run(object o)
        {
            this.Destroy((T)o);
        }
        
        public abstract void Destroy(T self);
    }
}