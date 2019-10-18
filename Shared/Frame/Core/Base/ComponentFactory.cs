using System;
using RDHelper;

namespace Frame.Core.Base
{
    public static class ComponentFactory
    {
	    private static T Generate<T>(AComponent parent) where T : AComponent
	    {
		    var type = typeof (T);
			
		    var component=(T)Activator.CreateInstance(type);
		    component.Id = IdGenerateHelper.GenerateComponentId();
		    component.Parent = parent;
	        
		    App.Inst.EventSystem.AddComponent(component);
		    return component;
	    }

        public static T Create<T>(AComponent parent) where T : AComponent
        {
			var component = Generate<T>(parent);
            App.Inst.EventSystem.Awake(component);
            return component;
        }
        
        
	    public static T Create<T, A>(AComponent parent, A a) where T : AComponent
	    {
		    var component = Generate<T>(parent);
		    App.Inst.EventSystem.Awake(component,a);
		    return component;
	    }
	    
	    
	    public static T Create<T, A,B>(AComponent parent, A a,B b) where T : AComponent
	    {
		    var component = Generate<T>(parent);
		    App.Inst.EventSystem.Awake(component,a,b);
		    return component;
	    }
	    
	    public static T Create<T, A,B,C>(AComponent parent, A a,B b,C c) where T : AComponent
	    {
		    var component = Generate<T>(parent);
		    App.Inst.EventSystem.Awake(component,a,b,c);
		    return component;
	    }
    }
}