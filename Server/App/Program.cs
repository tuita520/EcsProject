using System;
using Frame.Core.Base;
using Frame.Core.Network;
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
//                Options options = App.Inst.AddComponent<OptionComponent, string[]>(args).Options;
                
//                App.Inst.AddComponent<PlayerManagerComponent,bool>(true);
                App.Inst.AddComponent<NetworkComponent,NetworkProtocol,NetworkType,string,bool>(NetworkProtocol.TCP ,NetworkType.Listener,"127.0.0.1:50000",false);
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
