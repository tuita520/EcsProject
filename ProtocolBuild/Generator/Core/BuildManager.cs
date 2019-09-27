using Utility;

namespace ProtocolBuild.Generator.Core
{
    public class BuildManager:Singleton<BuildManager>
    {
        public void Run()
        {
            CodeFileManager.Inst.LoadFile("");
            foreach (var item in CodeFileManager.Inst.CodeFiles)
            {
                ProtoFileManager.Inst.LoadCodeFile(item.Value);
            }

            foreach (var item in ProtoFileManager.Inst.ProtoFiles)
            {
                
            }
            
        }
    }
}