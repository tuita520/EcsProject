/************************************************************************ 
 * 项目名称 :  ProtocolGenerator       
 * 类 名 称 :  FileUtil 
 * 类 描 述 :  文件操作工具类
 * 版 本 号 :  v1.0.0.0  
 * 说    明 :      
 * 作    者 :  Boiling 
 * 创建时间 :  2018/4/20 星期五 18:06:35 
 * 更新时间 :  2018/4/20 星期五 18:06:35 
************************************************************************ 
 * Copyright @ BoilingBlood 2018. All rights reserved. 
************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Utility
{
    public class FileUtil
    {
        public static bool WriteToFile(StringBuilder fileContent, string fileFullName)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(fileFullName);
                FileStream fileStream = null;
                if (!fileInfo.Exists)
                {
                    if (fileInfo.Directory != null && !fileInfo.Directory.Exists)
                    {
                        Directory.CreateDirectory(fileInfo.Directory.FullName);
                    }
                    fileStream = fileInfo.Create();
                }
                else
                {
                    fileStream = fileInfo.OpenWrite();
                }

                if (fileContent != null) fileStream.Write(Encoding.Default.GetBytes(fileContent.ToString()));
                fileStream.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        public static FileInfo[] FindFiles(string path,string key)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if (directoryInfo .Exists)
            {
                FileInfo[] files = directoryInfo. GetFiles(key,SearchOption.AllDirectories);
                return files;
            }
            return null;
//            string[] files = Directory.GetFiles(path, "*.code", SearchOption.AllDirectories);
//            string[] files = Directory.GetFiles(path, key, SearchOption.AllDirectories);
        }

        public static string GetFileHashValue(string fileFullName)
        {
            //计算第一个文件的哈希值
            var hash = System.Security.Cryptography.HashAlgorithm.Create();
            var stream = new System.IO.FileStream(fileFullName, System.IO.FileMode.Open);
            byte[] hashByte = hash.ComputeHash(stream);
            stream.Close();
            string hashValue = BitConverter.ToString(hashByte);
            return hashValue;
        }

        public static string ReadFromFile(string filePath, string fileName, string fileSuffix)
        {
            try
            {
                string fileFullName = filePath + fileName + fileSuffix;

                FileInfo fi = new FileInfo(fileFullName);
                if (fi.Exists)
                {
                    FileStream fsRead = fi.OpenRead();
                    StreamReader sr = new StreamReader(fsRead);
                    string s = sr.ReadLine();
                    sr.Close();
                    fsRead.Close();
                    return s;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }         
            return null;
        }

        public static List<string> ReadLinesFromFile(FileInfo fi)
        {
            try
            {
                var lines = new List<string>();
                var fsRead = fi.OpenRead();
                var sr = new StreamReader(fsRead);
                var s = "";
                do
                {
                    s = sr.ReadLine();
                    if (s != null)
                    {
                        lines.Add(s);
                    }
                } while (s != null);

                sr.Close();
                fsRead.Close();
                return lines;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        public static List<string> ReadLinesFromFile(string fileFullName)
        {
            try
            {
                FileInfo fi = new FileInfo(fileFullName);
                if (!fi.Exists)
                {
                    return null;
                }
                List<string> lines = new List<string>();

                FileStream fsRead = fi.OpenRead();
                StreamReader sr = new StreamReader(fsRead);
                string s = "";
                do
                {
                    s = sr.ReadLine();
                    if (s != null)
                    {
                        lines.Add(s);
                    }
                } while (s != null);

                sr.Close();
                fsRead.Close();
                return lines;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

    }
}
