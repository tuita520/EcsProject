using Frame.Core.Base;
using Frame.Core.Network;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            App.Inst.Init();
            App.Inst.AddDll(DLLType.Frame,typeof(App).Assembly);
            App.Inst.AddComponent<NetworkConnecterComponent,NetworkProtocol,string>(NetworkProtocol.TCP ,"192.168.1.108:50000");
            App.Inst.Run();
        }
    }
}
