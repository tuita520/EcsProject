using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utility;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            FileUtil.WriteToFile(new StringBuilder("2"), @"..\TestFile\ServerShard.code");
        }
    }
}
