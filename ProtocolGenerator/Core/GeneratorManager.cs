using Utility;

namespace ProtocolGenerator.Core
{
    public class GeneratorManager:Singleton<GeneratorManager>
    {
        public void Run()
        {
            CodeFileManager.Inst.LoadFiles(ConstData.PROTOCOL_PATH);
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