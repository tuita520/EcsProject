using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class TestString
    {
        [TestMethod]
        public void TestStringFormat()
        {
            string syntax = @"""proto3""";
            string a = string.Format($"syntax = {syntax}");
            int compare = string .Compare("11", "11");
            Console.WriteLine(a);
        }
    }
}