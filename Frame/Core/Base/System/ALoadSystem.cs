using System;

namespace Frame.Core.Base
{
    public interface ILoadSystem
    {
        Type Type();
        void Run(object o);
    }

    
    public abstract class ALoadSystem<T>:ILoadSystem
    {
        public Type Type()
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