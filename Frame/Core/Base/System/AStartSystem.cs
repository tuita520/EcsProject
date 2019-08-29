using System;

namespace Frame.Core.Base
{
    public abstract class AStartSytem<T>:ISystem
    {
        public new Type GetType()
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