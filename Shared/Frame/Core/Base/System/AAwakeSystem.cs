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
    
    public interface IAwakeRun<A,B,C>
    {
        void Run(object o, A a,B b,C c);
    }
    
    public interface IAwakeRun<A,B,C,D>
    {
        void Run(object o, A a,B b,C c,D d);
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
    
    public abstract class AAwakeSystem<T, A,B,C> : IAwakeSystem,IAwakeRun<A,B,C>
    {
        public Type Type()
        {
            return typeof(T);
        }

        public void Run(object o, A a,B b,C c)
        {
            Awake((T)o, a,b,c);
        }

        protected abstract void Awake(T self, A a,B b,C c);
    }
    
    public abstract class AAwakeSystem<T, A,B,C,D> : IAwakeSystem,IAwakeRun<A,B,C,D>
    {
        public Type Type()
        {
            return typeof(T);
        }

        public void Run(object o, A a,B b,C c,D d)
        {
            Awake((T)o, a,b,c,d);
        }

        protected abstract void Awake(T self, A a,B b,C c,D d);
    }
}