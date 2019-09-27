using System;
using System.Collections.Generic;
using ProtocolGenerator.Core;
using Utility;

namespace ProtocolBuild.Generator.Core
{
    public class ProtoFileManager
    {
        Dictionary<string,ProtoFile> protoFiles = new Dictionary<string, ProtoFile>();

        public void LoadCodeFile(CodeFile codeFile)
        {
            var protoFileNameList = GetProtoFileNameList(codeFile.PackageType);
            foreach (var fileName in protoFileNameList)
            {
                if (!protoFiles.TryGetValue(fileName,out var protoFile))
                {
                    protoFile = new ProtoFile();
                    protoFile.SetFileName(fileName);
                    protoFile.SetSyntax(codeFile.Syntax);
                    
                    protoFiles.Add(fileName,protoFile);
                }
                protoFile.LoadCodeFile(codeFile);
            }
        }

        private IEnumerable<string> GetProtoFileNameList(PackageType codeFilePackageType)
        {
            var fileNameList = new List<string>();
            switch (codeFilePackageType)
            {
                case PackageType.Client:
                    fileNameList.Add( ConstData.FILE_NAME_CLIENT_PROTO);
                    break;
                case PackageType.Server:
                    fileNameList.Add( ConstData.FILE_NAME_SERVER_PROTO);
                    break;
                case PackageType.Both:
                    fileNameList.Add( ConstData.FILE_NAME_CLIENT_PROTO);
                    fileNameList.Add( ConstData.FILE_NAME_SERVER_PROTO);
                    break;
            }
            return fileNameList;
        }
        
    }
}