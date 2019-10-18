using Frame.Core.Enumerate;

namespace Frame.Config
{
    public class Options
    {
        public int AreaId { get; set; }
        public int SubId { get; set; }
		
        // 没啥用，主要是在查看进程信息能区分每个app.exe的类型
        public AppType AppType { get; set; }
    }

}