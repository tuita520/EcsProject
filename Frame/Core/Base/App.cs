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
            _eventSystem = new EventSystem();
            _zone = new ZoneEntity();

            _zone.AddComponent<NetworkComponent, string>("127.0.0.1:");
        }

        public K AddComponent<K>() where K : AComponent, new()
        {
           return _zone.AddComponent<K>();
        }

        public void Run()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(1);
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