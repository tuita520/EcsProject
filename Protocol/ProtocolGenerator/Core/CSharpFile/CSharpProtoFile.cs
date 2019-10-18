using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using RDLog;
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

            string outPutfile="./";
            var outPutDirList = new List<string>();
            if (protoFile.Option.Contains("Client"))
            {
                outPutfile = fileInfo.Directory.FullName.Replace(@"\Proto\", @"\Client\");
                outPutDirList.Add(outPutfile);
            }
            outPutfile = fileInfo.Directory.FullName.Replace(@"\Proto\", @"\Server\");
            outPutDirList.Add(outPutfile);

            foreach (var outPutDir in outPutDirList)
            {
                var outPutDirectory = new DirectoryInfo(outPutDir);
                if (!outPutDirectory.Exists)
                {
                    outPutDirectory.Create();
                }

                //调用外部程序protoc.exe
                CmdProcessUtil.RunCmdProcess($@"{FolderManager.Inst.GoogleGeneratorFileDirectory}\protoc.exe", $@"-I . --csharp_out={outPutDir} {protoFile.Name}",FolderManager.Inst.CurrentDirectory);
            }
        }

        private void CSharpHandlerFile()
        {
        }
    }
}