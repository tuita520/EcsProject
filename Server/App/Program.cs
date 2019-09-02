using System;
using Frame.Core.Base;
using RDLog;
using Server.Core.Network;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                App.Inst.Init();
                App.Inst.AddDll(DLLType.Frame,typeof(App).Assembly);
                App.Inst.AddComponent<NetworkListenerComponent,NetworkProtocol,string>(NetworkProtocol.TCP,"127.0.0.1:50000");
                App.Inst.Run();
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
        }
    }
}
