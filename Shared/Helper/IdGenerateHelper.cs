namespace RDHelper
{
    public static class IdGenerateHelper
    {
        private static long appId;
        private static long componentId;
        
        public static long GenerateComponentId()
        {
            return ++componentId;
        }
    }
}