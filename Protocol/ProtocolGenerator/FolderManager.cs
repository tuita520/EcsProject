using System.IO;
using ProtocolGenerator.Core;
using RDLog;
using Utility;

namespace ProtocolGenerator
{
    public class FolderManager:Singleton<FolderManager>
    {
        public string CurrentDirectory = string.Empty;
        public string BinDirectory = string.Empty;
        
        public string GoogleGeneratorFileDirectory = string.Empty;

        public string ProtocolFileDirectory = string.Empty;

        public string CodeFilesDir = string.Empty;
        public string ProtoFilesDir = string.Empty;
        public string CSharpFilesDir = string.Empty;
        
        public void Init(string currentDirectory)
        {
            this.CurrentDirectory = currentDirectory;
            var curDirectoryInfo = new DirectoryInfo(CurrentDirectory);
            if (curDirectoryInfo.Parent!=null)
            {
                BinDirectory = curDirectoryInfo.Parent.FullName;
            }
            else
            {
                Log.Error($"please check bin directory:({currentDirectory})");
                return;
            }

            GoogleGeneratorFileDirectory = DirectoryUtil.PathCombine(BinDirectory, ConstData.PROTOC_DIR);
        }

        public bool InitWorkFileDir(string inputDir)
        {
            CodeFilesDir = inputDir;
            var codeDirInfo = new DirectoryInfo(CodeFilesDir);
            if (codeDirInfo.Parent != null)
            {
                ProtocolFileDirectory = codeDirInfo.Parent.FullName;
            }
            else
            {
                Log.Error($"please check protocol file directory folder:({inputDir})");
                return false;
            }
            
            ProtoFilesDir = DirectoryUtil.PathCombine(ProtocolFileDirectory, ConstData.PROTO_FILE_FOLDER);
            CSharpFilesDir = DirectoryUtil.PathCombine(ProtocolFileDirectory, ConstData.CSHARP_FILE_FOLDER);
            return true;
        }

        public void PrintAllDirs()
        {
            Log.Debug("------------------------------");
            Log.Debug($"{CurrentDirectory}");
            Log.Debug("------------------------------");

            Log.Debug($"{BinDirectory}");
            Log.Debug($"{GoogleGeneratorFileDirectory}");
            Log.Debug($"{ProtocolFileDirectory}");
            Log.Debug($"{CodeFilesDir}");
            Log.Debug($"{ProtoFilesDir}");
            Log.Debug($"{CSharpFilesDir}");
        }
    }
}