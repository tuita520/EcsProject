using System;
using System.Threading;
using RDLog;
using Server.Core.Network;
using Utility;

namespace Frame.Core.Base
{
    public class App:Singleton<App>
    {
        private EventSystem _eventSystem;

        public EventSystem EventSystem => _eventSystem;
        
        private ZoneEntity _zone;

        public ZoneEntity Zone => _zone ;

        public void Init()
        {
            // 异步方法全部会回掉到主线程
            SynchronizationContext.SetSynchronizationContext(MainThreadSynchronizationContext.Inst);
            
            _eventSystem = new EventSystem();
            _zone = new ZoneEntity();
        }


        public T AddComponent<T>() where T : AComponent, new()
        {
           return _zone.AddComponent<T>();
        }

        public T AddComponent<T,A>(A a) where T : AComponent, new()
        {
            return _zone.AddComponent<T,A>(a);
        }
        
        public void AddSystem<T>()
        {
            _eventSystem.AddSystem<T>();
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