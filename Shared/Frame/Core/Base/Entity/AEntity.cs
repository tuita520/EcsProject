using System;
using System.Collections.Generic;

namespace Frame.Core.Base
{
    public abstract class AEntity:AComponent
    {
        Dictionary<Type,AComponent> componentDict = new Dictionary<Type, AComponent>();

//        public ACompenent AddCompenent(ACompenent compenent)
//        {
//            if (compenentDict.ContainsKey(compenent.Id))
//            {
//                throw new Exception($"AddComponent, component already exist, id: {this.Id}, component: {compenent.GetType().Name}");
//            }
//            return compenent;
//        }
        
        public virtual K AddComponent<K>() where K : AComponent, new()
        {
            Type type = typeof (K);
            if (this.componentDict.ContainsKey(type))
            {
                throw new Exception($"AddComponent, component already exist, id: {this.Id}, component: {typeof(K).Name}");
            }

            K component = ComponentFactory.Create<K>(this);
            this.componentDict.Add(type, component);
            return component;
        }
        
        public virtual K AddComponent<K, A>(A a) where K :AComponent, new()
        {
            Type type = typeof (K);
            if (this.componentDict.ContainsKey(type))
            {
                throw new Exception($"AddComponent, component already exist, id: {this.Id}, component: {typeof(K).Name}");
            }

            K component = ComponentFactory.Create<K, A>(this, a);
			
            componentDict.Add(type, component);
            return component;
        }
    
        public virtual K AddComponent<K, A,B>(A a,B b) where K :AComponent, new()
        {
            Type type = typeof (K);
            if (this.componentDict.ContainsKey(type))
            {
                throw new Exception($"AddComponent, component already exist, id: {this.Id}, component: {typeof(K).Name}");
            }

            K component = ComponentFactory.Create<K,A,B>(this, a,b);
			
            componentDict.Add(type, component);
            return component;
        }
        
        public virtual K AddComponent<K, A,B,C>(A a,B b,C c) where K :AComponent, new()
        {
            Type type = typeof (K);
            if (this.componentDict.ContainsKey(type))
            {
                throw new Exception($"AddComponent, component already exist, id: {this.Id}, component: {typeof(K).Name}");
            }

            K component = ComponentFactory.Create<K,A,B,C>(this, a,b,c);
			
            componentDict.Add(type, component);
            return component;
        }
        

        
        public virtual void RemoveComponent<K>() where K : AComponent
        {
            if (this.IsDisposed)
            {
                return;
            }
            Type type = typeof (K);
            AComponent component;
            if (!this.componentDict.TryGetValue(type, out component))
            {
                return;
            }
            this.componentDict.Remove(type);
            component.Dispose();
        }

    }


}