using System;
using RDHelper;

namespace Core.Frame
{
    public static class ComponentFactory
    {
        public static T Create<T>() where T : AComponent
        {
            var type = typeof (T);
			
            var component=(T)Activator.CreateInstance(type);
            component.Id = IdGenerateHelper.GenerateComponentId();
            return component;
        }
        
    }
}