using System;

namespace Frame.Core.Base
{
    
    public interface IUpdateSystem
    {
        Type Type();
        void Run(object o);
    }
    
    public abstract class AUpdateSystem<T>:IUpdateSystem
    {
        public Type Type()
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