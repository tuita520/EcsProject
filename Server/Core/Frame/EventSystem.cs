using System;
using System.Collections.Generic;
using System.ComponentModel;
using RDHelper;
using RDLog;
using Server.Core.Utility;

namespace Core.Frame
{
    public sealed class EventSystem
    {
        private readonly Dictionary<long, AComponent> allComponents = new Dictionary<long, AComponent>();
        
        private readonly UnOrderMultiMap<Type, ISystem> startSystems = new UnOrderMultiMap<Type, ISystem>();
        private readonly UnOrderMultiMap<Type, ISystem> destroySystems = new UnOrderMultiMap<Type, ISystem>();
        
        private readonly UnOrderMultiMap<Type, ISystem> loadSystems = new UnOrderMultiMap<Type, ISystem>();
        
        private readonly UnOrderMultiMap<Type, ISystem> updateSystems = new UnOrderMultiMap<Type, ISystem>();
        private readonly UnOrderMultiMap<Type, ISystem> lateUpdateSystems = new UnOrderMultiMap<Type, ISystem>();
        
        
        private readonly Queue<long> starts = new Queue<long>();

        private Queue<long> loaders = new Queue<long>();
        private Queue<long> loaders2 = new Queue<long>();
        
        private Queue<long> updates = new Queue<long>();
        private Queue<long> updates2 = new Queue<long>();
        
        private Queue<long> lateUpdates = new Queue<long>();
        private Queue<long> lateUpdates2 = new Queue<long>();
        
        public void Add(AComponent component)
        {
            this.allComponents.Add(component.Id, component);

            Type type = component.GetType();

            if (this.loadSystems.ContainsKey(type))
            {
                this.loaders.Enqueue(component.Id);
            }

            if (this.updateSystems.ContainsKey(type))
            {
                this.updates.Enqueue(component.Id);
            }

            if (this.startSystems.ContainsKey(type))
            {
                this.starts.Enqueue(component.Id);
            }

            if (this.lateUpdateSystems.ContainsKey(type))
            {
                this.lateUpdates.Enqueue(component.Id);
            }
        }

        public void Remove(long id)
        {
            this.allComponents.Remove(id);
        }
        
        public AComponent Get(long id)
        {
            AComponent component = null;
            this.allComponents.TryGetValue(id, out component);
            return component;
        }
        
        
        public void Load()
        {
            while (this.loaders.Count > 0)
            {
                var id = this.loaders.Dequeue();
                if (!this.allComponents.TryGetValue(id, out var component))
                {
                    continue;
                }
                if (component.IsDisposed)
                {
                    continue;
                }
				
                var iSystemList = this.loadSystems[component.GetType()];
                if (iSystemList == null)
                {
                    continue;
                }


                foreach (var iLoadSystem in iSystemList)
                {
                    try
                    {
                        iLoadSystem.Run(component);
                    }
                    catch (Exception e)
                    {
                        Log.Error(e);
                    }
                }
            }

            ObjectHelper.Swap(ref this.loaders, ref this.loaders2);
        }

        private void Start()
        {
            while (this.starts.Count > 0)
            {
                var id = this.starts.Dequeue();
                if (!this.allComponents.TryGetValue(id, out var component))
                {
                    continue;
                }

                var iStartSystems = this.startSystems[component.GetType()];
                if (iStartSystems == null)
                {
                    continue;
                }
				
                foreach (var iStartSystem in iStartSystems)
                {
                    try
                    {
                        iStartSystem.Run(component);
                    }
                    catch (Exception e)
                    {
                        Log.Error(e);
                    }
                }
            }
        }
        
        public void Destroy(Component component)
        {
            var iDestroySystems = this.destroySystems[component.GetType()];
            if (iDestroySystems == null)
            {
                return;
            }

            foreach (var iDestroySystem in iDestroySystems)
            {
                if (iDestroySystem == null)
                {
                    continue;
                }

                try
                {
                    iDestroySystem.Run(component);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        public void Update()
        {
            this.Start();
			
            while (this.updates.Count > 0)
            {
                var id = this.updates.Dequeue();
                if (!this.allComponents.TryGetValue(id, out var component))
                {
                    continue;
                }
                if (component.IsDisposed)
                {
                    continue;
                }
				
                var iUpdateSystems = this.updateSystems[component.GetType()];
                if (iUpdateSystems == null)
                {
                    continue;
                }

                this.updates2.Enqueue(id);

                foreach (var iUpdateSystem in iUpdateSystems)
                {
                    try
                    {
                        iUpdateSystem.Run(component);
                    }
                    catch (Exception e)
                    {
                        Log.Error(e);
                    }
                }
            }

            ObjectHelper.Swap(ref this.updates, ref this.updates2);
        }

        public void LateUpdate()
        {
            while (this.lateUpdates.Count > 0)
            {
                var id = this.lateUpdates.Dequeue();
                if (!this.allComponents.TryGetValue(id, out var component))
                {
                    continue;
                }
                if (component.IsDisposed)
                {
                    continue;
                }

                var iLateUpdateSystems = this.lateUpdateSystems[component.GetType()];
                if (iLateUpdateSystems == null)
                {
                    continue;
                }

                this.lateUpdates2.Enqueue(id);

                foreach (var iLateUpdateSystem in iLateUpdateSystems)
                {
                    try
                    {
                        iLateUpdateSystem.Run(component);
                    }
                    catch (Exception e)
                    {
                        Log.Error(e);
                    }
                }
            }

            ObjectHelper.Swap(ref this.lateUpdates, ref this.lateUpdates2);
        }

    }
}