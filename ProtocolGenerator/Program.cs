using System;
using ProtocolGenerator.Core;

namespace ProtocolGenerator
{
    class Program
    {
        public static string InputFileDir = ConstData.INPUT_DIR_NAME;
        public static string ProtocDir;
        public static string RootDir;
        static void Main(string[] args)
        {
            RootDir = AppDomain.CurrentDomain.BaseDirectory;
            InputFileDir = $@"{RootDir}\..\..\..\{ConstData.INPUT_DIR_NAME}";
            ProtocDir = $@"{RootDir}\..\..\..\{ConstData.PROTOC_DIR}";
            GeneratorManager.Inst.Run();
//            CSharpFile.ProtoBuffFile(@"D:\GitHub\EcsProject\Protocol\Proto\Shared\Shared.proto");
          
            Console.WriteLine("Hello World!");
        }
    }
}