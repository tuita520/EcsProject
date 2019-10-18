using System.Collections.Generic;
using Utility;

namespace ProtocolGenerator.Core
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
                protoFile.SetPackage(codeFile.Package);
                protoFile.SetOption(codeFile.Option);
                ProtoFiles.Add(protoFile.Name, protoFile);
            }

            protoFile.LoadCodeFile(codeFile);
        }
        
    }
}