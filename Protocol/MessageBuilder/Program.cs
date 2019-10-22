using System;
using System.Diagnostics;
using Utility;

namespace Message
{
    class Program
    {
        static void Main(string[] args)
        {
            FolderManager.Inst.Init(Environment.CurrentDirectory);
            //调用外部程序protoc.exe
            CmdProcessUtil.RunCmdProcess($@"dotnet.exe", $@"{FolderManager.Inst.CurrentDirectory}\{ConstData.PROTOCOLGENERATOR} {FolderManager.Inst.CodeFilesDir}",FolderManager.Inst.CurrentDirectory);
//            var p = new Process();
//            p.StartInfo.FileName = "cmd.exe";
//            p.StartInfo.WorkingDirectory = FolderManager.Inst.CurrentDirectory;
//            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
//            p.StartInfo.RedirectStandardInput = false;//接受来自调用程序的输入信息
//            p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
//            p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
//            p.StartInfo.CreateNoWindow = true;//不显示程序窗口
//            p.StartInfo.Arguments = $@"dotnet {FolderManager.Inst.CurrentDirectory}\{Shared.EnumData.PROTOCOLGENERATOR} {FolderManager.Inst.CodeFilesDir}";
//            p.Start();
//            p.StandardInput.WriteLine("exit");
//            //获取cmd窗口的输出信息
//            string output = p.StandardOutput.ReadToEnd();
//            
//            p.WaitForExit();//等待程序执行完退出进程
//            p.Close();
        }
    }
}