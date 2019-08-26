using System;
using RDHelper;

namespace Core.Frame
{
    public static class ComponentFactory
    {
        public static T Create<T>() where T : AComponent
        {
            Type type = typeof (T);
			
            T component=(T)Activator.CreateInstance(type);
            component.Id = IdGenerateHelper.GenerateComponentId();
            return component;
        }
        
    }
}