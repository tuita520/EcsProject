using System.Collections.Generic;
using ProtocolGenerator.Core;
using Utility;

namespace ProtocolBuild.Generator.Core
{
    public class ProtoFileManager : Singleton<ProtoFileManager>
    {
        public Dictionary<string, ProtoFile> ProtoFiles = new Dictionary<string, ProtoFile>();

        public void LoadCodeFile(CodeFile codeFile)
        {
            if (!ProtoFiles.TryGetValue(codeFile.Name, out var protoFile))
            {
                protoFile = new ProtoFile();
                protoFile.SetFileName(codeFile.Name);
                protoFile.SetSyntax(codeFile.Syntax);
                ProtoFiles.Add(protoFile.Name, protoFile);
            }

            protoFile.LoadCodeFile(codeFile);
        }
        
    }
}