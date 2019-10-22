using System;
using System.IO;
using EnumData;
using RDLog;
using Utility;

namespace Server
{
    public class FolderManager:Singleton<FolderManager>
    {
        public string CurrentDirectory = string.Empty;
        public string BinDirectory = string.Empty;
        public string ServerDirectory = string.Empty;
        public string ConfigDirectory= string.Empty;

        /// <summary>
        /// 初始化项目目录
        /// </summary>
        /// <param name="currentDirectory"> Environment.CurrentDirectory(该进程从中启动的目录)</param>
        public void Init(string currentDirectory)
        {
            this.CurrentDirectory = currentDirectory;
            var curDirectoryInfo = new DirectoryInfo(CurrentDirectory);
            if (curDirectoryInfo.Parent!=null)
            {
                ServerDirectory = curDirectoryInfo.Parent.FullName;
            }
            else
            {
                Log.Error($"please check directory server folder {currentDirectory} ");
                return;
            }
            
            var serverDirectoryInfo = new DirectoryInfo(ServerDirectory);
            if (serverDirectoryInfo.Parent!=null)
            {
                BinDirectory = serverDirectoryInfo.Parent.FullName;
            }
            else
            {
                Log.Error($"please check directory  bin folder {serverDirectoryInfo}");
                return;
            }
            ConfigDirectory =DirectoryUtil.PathCombine(BinDirectory, CommonConst.ConfigFolderName);
            var configDirectoryInfo = new DirectoryInfo(ConfigDirectory);
            if (!configDirectoryInfo.Exists)
            {
                configDirectoryInfo.Create();
            }
            
            
        }

    }
}