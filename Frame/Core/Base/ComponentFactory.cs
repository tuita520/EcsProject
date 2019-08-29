using System;
using RDHelper;

namespace Frame.Core.Base
{
    public static class ComponentFactory
    {
        public static T Create<T>(AComponent parent = null) where T : AComponent
        {
            var type = typeof (T);
			
            var component=(T)Activator.CreateInstance(type);
            component.Id = IdGenerateHelper.GenerateComponentId();
            component.Parent = parent;
            App.Inst.EventSystem.Add(component);
            App.Inst.EventSystem.Awake(component);
            return component;
        }
        
        
	    public static T Create<T, A>(AComponent parent, A a) where T : AComponent
	    {
		    var type = typeof (T);
		    var component=(T)Activator.CreateInstance(type);
		    component.Id = IdGenerateHelper.GenerateComponentId();
		    component.Parent = parent;
	
		    App.Inst.EventSystem.Add(component);
		    App.Inst.EventSystem.Awake(component,a);
		    return component;
	    }
    }
}