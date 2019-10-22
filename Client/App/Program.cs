using Client.Module;
using Frame.Core.Base;
using Frame.Core.Network;
using Game.Module.Server;
using RDHelper;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            App.Inst.Init();
            App.Inst.AddDll(DLLType.Frame, typeof(App).Assembly);
//            App.Inst.AddDll(DLLType.Module, DllHelper.GetClientModuleAssembly());

            App.Inst.AddComponent<ServerManager>();
            App.Inst.Run();
        }
    }
}