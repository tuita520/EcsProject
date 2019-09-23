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
   
    public interface IAwakeRun<A,B>
    {
        void Run(object o, A a,B b);
    }
	
    public abstract class AAwakeSystem<T>:IAwakeSystem,IAwakeRun
    {
        public Type Type()
        {
            return typeof(T);
        }
        
        public void Run(object o)
        {
            Awake((T)o);
        }

        protected abstract void Awake(T self);
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

        protected abstract void Awake(T self, A a);
    }
    
    public abstract class AAwakeSystem<T, A,B> : IAwakeSystem,IAwakeRun<A,B>
    {
        public Type Type()
        {
            return typeof(T);
        }

        public void Run(object o, A a,B b)
        {
            Awake((T)o, a,b);
        }

        protected abstract void Awake(T self, A a,B b);
    }
}