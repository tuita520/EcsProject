using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using RDLog;
using Utility;

namespace ProtocolBuild.Generator.Core
{
    public class CodeFileManager:Singleton<CodeFileManager>
    {
        //key :filename
        public readonly Dictionary<string, CodeFile> CodeFiles = new Dictionary<string, CodeFile>();
        
        private void AddCodeFile(CodeFile codeFile)
        {
            CodeFiles.Add(codeFile.Name,codeFile);
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
                    Log.Error($"LoadFile {fileFullName} error : file paraser wrong .");
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


    }
}