﻿using System;
using ProtocolGenerator.Core;

namespace ProtocolGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            BuildManager.Inst.Run();
//            CSharpFile.ProtoBuffFile(@"D:\GitHub\EcsProject\ProtocolBuild\Proto\Shared\Shared.proto");
          
            Console.WriteLine("Hello World!");
        }
    }
}