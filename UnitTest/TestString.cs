using System;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utility;

namespace UnitTest
{
    [TestClass]
    public class TestString
    {
        [TestMethod]
        public void TestStringFormat()
        {
            string syntax = @"  proto3   proto3  ";
            string a = string.Format($"syntax = {syntax}   ;");
            int compare = string .CompareOrdinal("11", "11");
            a = a.Replace(";", "").Trim();

           a = StringUtil.TrimStartWord(syntax, "proto3");

            a = Regex.Replace(syntax,@"^proto3\b","");
            var formatArr = syntax.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);

            Console.WriteLine(a);
        }
    }
}