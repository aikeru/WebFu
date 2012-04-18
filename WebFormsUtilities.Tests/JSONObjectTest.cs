using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebFormsUtilities.Json;

namespace WebFormsUtilities.Tests {
    /// <summary>
    /// Summary description for JSONObjectTest
    /// </summary>
    [TestClass]
    public class JSONObjectTest {
        [TestMethod()]
        public void JSONObject_RenderTests() {
            JSONObject obj = new JSONObject();
            Assert.AreEqual(obj.Render(), "null");
            obj.Value = new JSONObjectEmpty();
            Assert.AreEqual(obj.Render(), "{}");
            obj.Attr("propertyName", "propertyValue");
            Assert.AreEqual(obj.Attr("propertyName").Value, "propertyValue");
            Assert.AreEqual(obj.Render(), "{\"propertyName\":\"propertyValue\"}");
            obj = new JSONObject(new { property1 = "value1", property2 = 5 });
            Assert.AreEqual(obj.Render(), "{\"property1\":\"value1\",\"property2\":5}");

        }
    }
}
