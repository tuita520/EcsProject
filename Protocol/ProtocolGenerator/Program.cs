using System;
using System.Diagnostics;
using ProtocolGenerator.Core;
using RDLog;

namespace ProtocolGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Debug($"--> {args[0]}");
            FolderManager.Inst.Init(Environment.CurrentDirectory);

            if ((args.Length < 1))
            {
                Log.Error($@"generator fail ,check args ");
                return;
            }

            var inputDir = args[0];
//            var inputDir = @"D:\GitHub\EcsProject\Protocol\Code";
            if (!FolderManager.Inst.InitWorkFileDir(inputDir))
            {
                Log.Error($@"generator fail ,check directory {args[0]}");
                return;
            }
            GeneratorManager.Inst.Run();
            Console.Write("exit");
        }
    }
}