using Microsoft.VisualStudio.TestTools.UnitTesting;
using openhab.net.rest.Core;
using System;

namespace openhab.net.rest.test
{
    [TestClass]
    public class ValueParserTest
    {
        [TestMethod]
        public void TestValueParserInt()
        {
            int x;
            bool b = ValueParser.TryParse("1234", out x);
            Assert.IsTrue(b);
            Assert.AreEqual(x, 1234);
        }

        [TestMethod]
        public void TestValueParserFloat()
        {
            float f = 12.34f;
            float x;
            bool b = ValueParser.TryParse("12.34", out x);
            Assert.IsTrue(b);
            Assert.AreEqual(x, f);
        }

        [TestMethod]
        public void TestValueParserString()
        {
            var a = DateTime.Now.ToString();
            string x;
            bool b = ValueParser.TryParse("Test", out x);
            Assert.IsTrue(b);
            Assert.AreEqual(x, "Test");
        }

        [TestMethod]
        public void TestValueParserDateTime()
        {
            DateTime dt = new DateTime(2016, 7, 15, 12, 30, 44);
            DateTime x;
            bool b = ValueParser.TryParse("2016-07-15T12:30:44", out x);
            Assert.IsTrue(b);
            Assert.IsTrue(x == dt);
        }
    }
}
