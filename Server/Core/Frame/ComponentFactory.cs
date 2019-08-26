using System;

namespace Core.Frame
{
    public static class ComponentFactory
    {
        private static long maxId;
        public static T Create<T>() where T : AComponent
        {
            Type type = typeof (T);
			
            T component=(T)Activator.CreateInstance(type);
            component.Id = maxId;
            return component;
        }
        
    }
}