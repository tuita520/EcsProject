using System;

namespace Core.Frame
{
    public abstract class ALateUpdateSystem<T>:ISystem
    {
        public new Type GetType()
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