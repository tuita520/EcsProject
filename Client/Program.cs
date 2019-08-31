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
            App.Inst.AddSystem<NetworkComponentAwakeSystem>();
            App.Inst.AddComponent<NetworkComponent,string>("127.0.0.1:50000");
            App.Inst.Run();
        }
    }
}
