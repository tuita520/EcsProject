using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using RDLog;
using Utility;

namespace Frame.Core.Base
{
    public class App:Singleton<App>
    {
        private EventSystem _eventSystem;

        public EventSystem EventSystem => _eventSystem;
        
        private Zone _zone;

        public Zone Zone => _zone ;

        public void Init()
        {
            // 异步方法全部会回掉到主线程
            SynchronizationContext.SetSynchronizationContext(MainThreadSynchronizationContext.Inst);
            
            _eventSystem = new EventSystem();
            _zone = new Zone();
        }

        public void  AddDll(DLLType dllType, Assembly assembly)
        {
            _eventSystem.AddDll(dllType, assembly);
        }

        public T AddComponent<T>() where T : AComponent, new()
        {
           return _zone.AddComponent<T>();
        }

        public T AddComponent<T,A>(A a) where T : AComponent, new()
        {
            return _zone.AddComponent<T,A>(a);
        }
        
        public T AddComponent<T,A,B>(A a,B b) where T : AComponent, new()
        {
            return _zone.AddComponent<T,A,B>(a,b);
        }
        
        public T AddComponent<T,A,B,C>(A a,B b,C c) where T : AComponent, new()
        {
            return _zone.AddComponent<T,A,B,C>(a,b,c);
        }

        public T AddComponent<T,A,B,C,D>(A a,B b,C c,D d) where T : AComponent, new()
        {
            return _zone.AddComponent<T,A,B,C,D>(a,b,c,d);
        }
        
        public void Run()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(1);
                    MainThreadSynchronizationContext.Inst.Update();
                    _eventSystem.Update();
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        public void Close()
        {
            
        }


    }
}