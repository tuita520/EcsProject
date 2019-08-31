using System;

namespace Frame.Core.Base
{
    public interface IAwakeSystem
    {
        Type Type();
    }
    
    public interface IAwakeRun
    {
        void Run(object o);
    }
    
    public interface IAwakeRun<A>
    {
        void Run(object o, A a);
    }
	
    public abstract class AAwakeSystem<T>:IAwakeSystem,IAwakeRun
    {
        public new Type Type()
        {
            return typeof(T);
        }
        
        public void Run(object o)
        {
            this.Awake((T)o);
        }
        
        public abstract void Awake(T self);
    }
    
    
    public abstract class AAwakeSystem<T, A> : IAwakeSystem,IAwakeRun<A>
    {
        public Type Type()
        {
            return typeof(T);
        }

        public void Run(object o, A a)
        {
            this.Awake((T)o, a);
        }

        public abstract void Awake(T self, A a);
    }
}