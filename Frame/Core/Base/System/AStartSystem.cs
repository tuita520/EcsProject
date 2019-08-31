using System;

namespace Frame.Core.Base
{
    public interface IStartSystem
    {
        Type Type();
        void Run(object o);
    }
    
    public abstract class AStartSystem<T>:IStartSystem
    {
        public Type Type()
        {
            return typeof(T);
        }

        public void Run(object o)
        {
            this.Start((T)o);
        }
        
        public abstract void Start(T self);
    }
}