﻿using Frame.Core.Base;
using RDLog;
using Server.Core.NetWork;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            App.Inst.Init();
            App.Inst.Run();
            Log.Info("..........................");
        }
    }
}
