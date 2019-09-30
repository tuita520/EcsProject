using System.Diagnostics;

namespace ProtocolBuild
{
    class Program
    {
        static void Main(string[] args)
        {
            ProtoBuffFile(@"D:\GitHub\EcsProject\Protocol\Proto\Shared\");
        }
        
        public static void ProtoBuffFile(string protoFilesPath)
        {
            //调用外部程序protogen.exe
            var p = new Process
            {
                StartInfo =
                {
                    FileName = @"D:\GitHub\ProtocolGenerator\Output\net35\google_protoc\protoc.exe",
                    WorkingDirectory = protoFilesPath,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = false,
                    Arguments = $@"-I . --csharp_out=D:\GitHub\EcsProject\ServerProtocol Shard.proto" 
                }
            };
//            p.StartInfo.FileName = "cmd.exe";


//            p.StartInfo.Arguments = @" -I" + m_filePathIn+ " *.proto" + @" --csharp_out=" + m_filePathOut;// -ns:" + Application.StartupPath;
//            p.StartInfo.Arguments = @" -i:" + m_InputfilePath+ m_InputfileSimpleName + " -o:" + m_OutputfilePath + m_InputfileName+@".cs";
//            p.StartInfo.Arguments = m_InputfileSimpleName+ @" -I" + m_InputfilePath + @" --csharp_out=" + m_OutputfilePath;
//            Console.WriteLine("{0} {1}", p.StartInfo.FileName, p.StartInfo.Arguments);

            p.Start();
            p.StandardInput.WriteLine("exit");

//            p.WaitForExit();
//            string command = Application.StartupPath + @"\protogen.exe -i:" + fileName + @" -o:descriptor.cs -ns:" + Application.StartupPath;
//            p.StandardInput.WriteLine(command);
//            p.StandardInput.WriteLine("exit"); //需要有这句，不然程序会挂机
//             向cmd.exe输入command
//            string output = p.StandardOutput.ReadToEnd(); 这句可以用来获取执行命令的输出结果
        }
    }
}
