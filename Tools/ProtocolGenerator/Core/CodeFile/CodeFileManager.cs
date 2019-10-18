using System;
using System.Collections.Generic;
using System.IO;
using RDLog;
using Utility;

namespace ProtocolGenerator.Core
{
    public class CodeFileManager:Singleton<CodeFileManager>
    {
        //key :filename
        public readonly Dictionary<string, CodeFile> CodeFiles = new Dictionary<string, CodeFile>();
        
        private void AddCodeFile(CodeFile codeFile)
        {
            CodeFiles.Add(codeFile.Name,codeFile);
        }

        private void LoadFile(FileInfo fileInfo)
        {
            var parser = new CodeFileParser();
            try
            {
                var codeFile =  parser.ParserFile(fileInfo);
                if (codeFile == null)
                {
                    return;
                }

                AddCodeFile(codeFile);
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
        }
        
        public void LoadFile(string fileFullName)
        {
             CodeFileParser parser = new CodeFileParser();
            if (fileFullName == null)
            {
                Log.Error("ParseCodeFile error : fileFullName is null.");
                return;
            }

            try
            {
                CodeFile codeFile =  parser.ParserFile(fileFullName);

                if (codeFile == null)
                {
                    return;
                }

                AddCodeFile(codeFile);
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
        }

        public void LoadFiles(string dir)
        {
           var files = FileUtil.FindFiles(dir, "*.code");
           if (files == null)
           {
               Log.Error($"load file fail : check file dir {dir}");
               return;
           }
           foreach (var file in files)
           {
               LoadFile(file);
           }
        }


    }
}