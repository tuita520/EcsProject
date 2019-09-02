using System;
using Frame.Core.Base;
using Server.Core.Network;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            App.Inst.Init();
            App.Inst.AddDll(DLLType.Frame,typeof(App).Assembly);
            App.Inst.AddComponent<NetworkConnecterComponent,NetworkProtocol,string>(NetworkProtocol.TCP ,"127.0.0.1:50000");
            App.Inst.Run();
        }
    }
}
