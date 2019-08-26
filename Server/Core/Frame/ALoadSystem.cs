using System;

namespace Core.Frame
{
    public abstract class ALoadSystem<T>:ISystem
    {
        public new Type GetType()
        {
            return typeof(T);
        }
        
        public void Run(object o)
        {
            this.Load((T)o);
        }
        
        public abstract void Load(T self);
    }
}