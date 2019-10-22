using System;
using Frame.Core.Base;
using Frame.Core.Network;
using Game.Module;
using Module;
using RDHelper;
using RDLog;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
//                ConfigDataManager.Inst.Init(new XmlParser());
//                ConfigDataManager.Inst.Parse(@"D:\code\EcsProject\Bin\Config\NetworkTopology.xml");
//
//                DataList dataList = ConfigDataManager.Inst.GetDataList("NetworkTopology");
//                foreach (var data in dataList)
//                {
//                    foreach (var attr in data.Value)
//                    {
//                        var at = attr.Value;
//                    }
//                }
                FolderManager.Inst.Init(Environment.CurrentDirectory);

                App.Inst.Init();
                App.Inst.AddDll(DLLType.Frame,typeof(App).Assembly);
                App.Inst.AddDll(DLLType.Module,DllHelper.GetServerModuleAssembly());
//                Options options = App.Inst.AddComponent<OptionComponent, string[]>(args).Options;
                App.Inst.AddComponent<ClientManager>();

//                App.Inst.AddComponent<PlayerManagerComponent,bool>(true);
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
