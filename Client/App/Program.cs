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
            App.Inst.AddComponent<NetworkComponent,NetworkProtocol,NetworkType,string,bool>(NetworkProtocol.TCP ,NetworkType.Connector,"127.0.0.1:50000",true);
            App.Inst.Run();
        }
    }
}
