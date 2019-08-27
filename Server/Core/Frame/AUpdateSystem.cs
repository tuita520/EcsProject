using System;

namespace Core.Frame
{
    public abstract class AUpdateSystem<T>:ISystem
    {
        public new Type GetType()
        {
            return typeof(T);
        }
        
        public void Run(object o)
        {
            this.Update((T)o);
        }
        
        public abstract void Update(T self);
    }
}