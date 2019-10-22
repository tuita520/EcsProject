using EnumData;
using Game.Module;

namespace Frame.Core.Network.Helper
{
    public class ComponentKeyHelper
    {
        /// <summary>
        /// 这个Key目前约定与server name 一致
        /// </summary>
        /// <param name="serverType"></param>
        /// <param name="areaId"></param>
        /// <param name="subId"></param>
        /// <returns></returns>
        public static string GetServerKey(ServerType serverType, int areaId,int subId)
        {
            var key = string.Format($"{serverType}_{areaId}_{subId}");
            return key ;
        }
                
    }
}