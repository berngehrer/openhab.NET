using Microsoft.VisualStudio.TestTools.UnitTesting;
using openhab.net.rest.Core;
using openhab.net.rest.Http;

namespace openhab.net.rest.test
{
    [TestClass]
    public class ExtensionTests
    {
        [TestMethod]
        public void TestAttributeExtensions()
        {
            var attribute = SiteCollection.Items.GetAttribute<FieldValueAttribute>();
            Assert.AreEqual(attribute?.Value, "/rest/items");
        }
    }
}
