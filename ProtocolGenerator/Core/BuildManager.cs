using ProtocolBuild.Generator.Core;
using Utility;

namespace ProtocolGenerator.Core
{
    public class BuildManager:Singleton<BuildManager>
    {
        public void Run()
        {
            CodeFileManager.Inst.LoadFiles("../../../ProtocolBuild");
            foreach (var item in CodeFileManager.Inst.CodeFiles)
            {
                ProtoFileManager.Inst.LoadCodeFile(item.Value);
            }

            foreach (var (_, value) in ProtoFileManager.Inst.ProtoFiles)
            {
                if (value.GenerateProtoFile())
                {
                    CSharpFile.ProtoBuffFile(value);
                }  
            }
            
            
        }
    }
}