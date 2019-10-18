using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Frame.Core.Base.Attributes;
using Frame.Core.Utility;
using RDHelper;
using RDLog;

namespace Frame.Core.Base
{
    
    public enum DLLType
    {
        Frame,
        Hotfix,
        Editor,
    }
    
    public sealed class EventSystem
    {
        private readonly Dictionary<DLLType, Assembly> assemblies = new Dictionary<DLLType, Assembly>();
        private readonly UnOrderMultiMap<Type, Type> types = new UnOrderMultiMap<Type, Type>();
        
        private readonly Dictionary<long, AComponent> allComponents = new Dictionary<long, AComponent>();
        
        private readonly UnOrderMultiMap<Type, IAwakeSystem> awakeSystems = new UnOrderMultiMap<Type, IAwakeSystem>();

        private readonly UnOrderMultiMap<Type, IStartSystem> startSystems = new UnOrderMultiMap<Type, IStartSystem>();
        private readonly UnOrderMultiMap<Type, IDestroySystem> destroySystems = new UnOrderMultiMap<Type, IDestroySystem>();

        private readonly UnOrderMultiMap<Type, ILoadSystem> loadSystems = new UnOrderMultiMap<Type, ILoadSystem>();

        private readonly UnOrderMultiMap<Type, IUpdateSystem> updateSystems = new UnOrderMultiMap<Type, IUpdateSystem>();
        private readonly UnOrderMultiMap<Type, ILateUpdateSystem> lateUpdateSystems = new UnOrderMultiMap<Type, ILateUpdateSystem>();


        private readonly Queue<long> starts = new Queue<long>();

        private Queue<long> loaders = new Queue<long>();
        private Queue<long> loaders2 = new Queue<long>();

        private Queue<long> updates = new Queue<long>();
        private Queue<long> updates2 = new Queue<long>();

        private Queue<long> lateUpdates = new Queue<long>();
        private Queue<long> lateUpdates2 = new Queue<long>();

        
        public void AddDll(DLLType dllType, Assembly assembly)
		{
			this.assemblies[dllType] = assembly;
			this.types.Clear();
			foreach (Assembly value in this.assemblies.Values)
			{
				foreach (Type type in value.GetTypes())
				{
					object[] objects = type.GetCustomAttributes(typeof(BaseAttribute), false);
					if (objects.Length == 0)
					{
						continue;
					}

					BaseAttribute baseAttribute = (BaseAttribute) objects[0];
					this.types.Add(baseAttribute.AttributeType, type);
				}
			}

			this.awakeSystems.Clear();
			this.lateUpdateSystems.Clear();
			this.updateSystems.Clear();
			this.startSystems.Clear();
			this.loadSystems.Clear();
			this.destroySystems.Clear();

			foreach (Type type in types[typeof(SystemAttribute)])
			{
				object[] attrs = type.GetCustomAttributes(typeof(SystemAttribute), false);

				if (attrs.Length == 0)
				{
					continue;
				}

				object obj = Activator.CreateInstance(type);

				switch (obj)
				{
					case IAwakeSystem objectSystem:
						this.awakeSystems.Add(objectSystem.Type(), objectSystem);
						break;
					case IUpdateSystem updateSystem:
						this.updateSystems.Add(updateSystem.Type(), updateSystem);
						break;
					case ILateUpdateSystem lateUpdateSystem:
						this.lateUpdateSystems.Add(lateUpdateSystem.Type(), lateUpdateSystem);
						break;
					case IStartSystem startSystem:
						this.startSystems.Add(startSystem.Type(), startSystem);
						break;
					case IDestroySystem destroySystem:
						this.destroySystems.Add(destroySystem.Type(), destroySystem);
						break;
					case ILoadSystem loadSystem:
						this.loadSystems.Add(loadSystem.Type(), loadSystem);
						break;
				}
			}

			this.Load();
		}
        
        
        public void AddComponent(AComponent component)
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

        public void AddSystem<T>()
        {
                object obj = Activator.CreateInstance(typeof(T));

                switch (obj)
                {
                    case IAwakeSystem objectSystem:
                        this.awakeSystems.Add(objectSystem.Type(), objectSystem);
                        break;
                    case IUpdateSystem updateSystem:
                        this.updateSystems.Add(updateSystem.Type(), updateSystem);
                        break;
                    case ILateUpdateSystem lateUpdateSystem:
                        this.lateUpdateSystems.Add(lateUpdateSystem.Type(), lateUpdateSystem);
                        break;
                    case IStartSystem startSystem:
                        this.startSystems.Add(startSystem.Type(), startSystem);
                        break;
                    case IDestroySystem destroySystem:
                        this.destroySystems.Add(destroySystem.Type(), destroySystem);
                        break;
                    case ILoadSystem loadSystem:
                        this.loadSystems.Add(loadSystem.Type(), loadSystem);
                        break;
            }

            this.Load();
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

        public void Awake(AComponent component)
        {
            var iAwakeSystems = this.awakeSystems[component.GetType()];
            if (iAwakeSystems == null)
            {
                return;
            }

            foreach (var aAwakeSystem in iAwakeSystems)
            {
                if (!(aAwakeSystem is IAwakeRun iAwake)) continue;
                try
                {
                    iAwake.Run(component);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        public void Awake<A>(AComponent component, A a)
        {
            var iAwakeSystems = this.awakeSystems[component.GetType()];
            if (iAwakeSystems == null)
            {
                return;
            }

            foreach (var aAwakeSystem in iAwakeSystems)
            {
                if (!(aAwakeSystem is IAwakeRun<A> iAwake)) continue;
                try
                {
                    iAwake.Run(component, a);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }
        
        public void Awake<A,B>(AComponent component, A a,B b)
        {
            var iAwakeSystems = this.awakeSystems[component.GetType()];
            if (iAwakeSystems == null)
            {
                return;
            }

            foreach (var aAwakeSystem in iAwakeSystems)
            {
                if (!(aAwakeSystem is IAwakeRun<A,B> iAwake)) continue;
                try
                {
                    iAwake.Run(component, a,b);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }
        
        public void Awake<A,B,C>(AComponent component, A a,B b,C c)
        {
            var iAwakeSystems = this.awakeSystems[component.GetType()];
            if (iAwakeSystems == null)
            {
                return;
            }

            foreach (var aAwakeSystem in iAwakeSystems)
            {
                if (!(aAwakeSystem is IAwakeRun<A,B,C> iAwake)) continue;
                try
                {
                    iAwake.Run(component, a,b,c);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
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

        public void Destroy(AComponent component)
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