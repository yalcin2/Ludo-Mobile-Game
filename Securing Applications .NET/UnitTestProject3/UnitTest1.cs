using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject3
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            BruteForce b = new BruteForce();
            b.Min = 2;
            b.Max = 4;
            b.charset = "abcdefghijklmnopqrstuvwxyz0123456789";
            string target = "abc1";
            foreach (string result in b)
            {
                Console.Write(result + "\r");
                if (result == target)
                {
                    Console.WriteLine("target found:" + result);
                    return;
                }
            }
        }
    }
}
