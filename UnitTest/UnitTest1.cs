using System;
using System.Globalization;
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
            string convertTime = "2012-12-12";
            DateTime mdate1 = DateTime.ParseExact(convertTime, "yyyy-MM-dd", null);
            Console.Write(1);
        }
        
        
    }
}
