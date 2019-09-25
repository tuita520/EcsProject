using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProtocolBuild.Generator.Core;

namespace UnitTest
{
    [TestClass]
    public class TestProtocolBuilder
    {
        [TestMethod]
        public void TestCodeFileParaser()
        {
            CodeFileManager.Inst.LoadFile(@"D:\code\EcsProject\ProtocolBuild\Code\ServerShared\ServerShard.code");
        }
    }
}