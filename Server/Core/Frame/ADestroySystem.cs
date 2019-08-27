using System;

namespace Core.Frame
{
    public abstract class ADestroySystem<T>:ISystem
    {
        public new Type GetType()
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