using System.IO;
using RDLog;
using Utility;

namespace Message
{
    public class FolderManager:Singleton<FolderManager>
    {
        public string CurrentDirectory = string.Empty;
        public string BinDirectory = string.Empty;
        public string ProtocolFileDirectory = string.Empty;
        public string CodeFilesDir = string.Empty;
        
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
            
            var binDirectoryInfo = new DirectoryInfo(BinDirectory);
            if (binDirectoryInfo.Parent!=null)
            {
                ProtocolFileDirectory = binDirectoryInfo.Parent.FullName;
            }
            else
            {
                Log.Error($"please check protocol file directory folder:({currentDirectory})");
                return;
            }

            CodeFilesDir = DirectoryUtil.PathCombine(ProtocolFileDirectory, ConstData.CODEFILESDIR);

        }

    }
}