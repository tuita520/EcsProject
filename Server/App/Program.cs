﻿using System;
using ConfigData;
using Frame.Config;
using Frame.Core.Base;
using Frame.Core.Enumerate;
using Frame.Core.Register;
using RDLog;
using Server.Core.Network;
using Utility;

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
                PathUtil.Init();

                App.Inst.Init();
                App.Inst.AddDll(DLLType.Frame,typeof(App).Assembly);
                Options options = App.Inst.AddComponent<OptionComponent, string[]>(args).Options;
                
//                App.Inst.AddComponent<PlayerManagerComponent,bool>(true);
//                App.Inst.AddComponent<NetworkListenerComponent,NetworkProtocol,string>(NetworkProtocol.TCP ,"192.168.1.108:50000");
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
