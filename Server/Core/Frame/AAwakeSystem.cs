using System;

namespace Core.Frame
{
    public abstract class AAwakeSystem<T>:ISystem
    {
        public new Type GetType()
        {
            return typeof(T);
        }
        
        public void Run(object o)
        {
            this.Awake((T)o);
        }
        
        public abstract void Awake(T self);
    }
}