using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Utility;

namespace ProtocolGenerator.Core
{
    public class CSharpFile
    {
        public string Name;
        public string FullName;
        public string Directory;
        
        public void ProtocolGenerator()
        {
                
        }

        public static void ProtoBuffFile(ProtoFile protoFile)
        {
            var fileInfo = new FileInfo(protoFile.FullName);
            if (!fileInfo.Exists)
            {
                return;
            }

            if (fileInfo.Directory == null)
            {
                return;
            }

            var outPutDirList = new List<string>();
            if (protoFile.Package.Contains("client"))
            {
                outPutDirList.Add(ConstData.CLIENTMSG_OUTPUT_PATH);
            }
            outPutDirList.Add(ConstData.SERVERMSG_OUTPUT_PATH);

            foreach (var outPutDir in outPutDirList)
            {
                var outPutDirectory = new DirectoryInfo(outPutDir);
                if (!outPutDirectory.Exists)
                {
                    outPutDirectory.Create();
                }

                //调用外部程序protoc.exe
                var p = new Process
                {
                    StartInfo =
                    {
                        FileName = @"D:\GitHub\EcsProject\ThirdParty\google_protoc\protoc.exe",
                        WorkingDirectory = fileInfo.Directory.FullName,
                        UseShellExecute = false,
                        CreateNoWindow = false,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = false,
                        Arguments = $@"-I . --csharp_out={outPutDir} {protoFile.Name}"
                    }
                };

                p.Start();
                p.StandardInput.WriteLine("exit");

//            p.WaitForExit();
//            p.StandardInput.WriteLine(command);
//            p.StandardInput.WriteLine("exit"); //需要有这句，不然程序会挂机
//            向cmd.exe输入command
//            string output = p.StandardOutput.ReadToEnd(); 这句可以用来获取执行命令的输出结果
            }
        }

        private void CSharpHandlerFile()
        {
        }
    }
}