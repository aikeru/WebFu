using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebFormsUtilities.Tests {
    /// <summary>
    /// Summary description for HtmlTagTest
    /// </summary>
    [TestClass]
    public class HtmlTagTest {

        [TestMethod()]
        public void HtmlTag_RenderTests() {
            HtmlTag tag = new HtmlTag("tagname", true);
            Assert.AreEqual(tag.Render(), "<tagname />");
            tag = new HtmlTag("tagname");
            Assert.AreEqual(tag.Render(), "<tagname></tagname>\r\n");
            tag.InnerText = "Sometext";
            Assert.AreEqual(tag.Render(), "<tagname>Sometext</tagname>\r\n");
            tag.Children.Add(new HtmlTag("tag2"));
            Assert.AreEqual(tag.Render(), "<tagname><tag2></tag2>\r\n</tagname>\r\n");
            tag.Attr("class", "someclass");
            Assert.AreEqual(tag.Render(), "<tagname class = \"someclass\"><tag2></tag2>\r\n</tagname>\r\n");
            tag.AddClass("newclass");
            Assert.AreEqual(tag.Render(), "<tagname class = \"someclass newclass\"><tag2></tag2>\r\n</tagname>\r\n");
            tag.RemoveClass("newclass");
            Assert.AreEqual(tag.Render(), "<tagname class = \"someclass\"><tag2></tag2>\r\n</tagname>\r\n");
            Assert.AreEqual(tag.Attr("class"), "someclass");

            Assert.AreEqual(tag.RenderBeginningOnly(), "<tagname class = \"someclass\">");
            Assert.AreEqual(tag.RenderEndingOnly(), "</tagname>\r\n");
            Assert.AreEqual(tag.IsClass("someclass"), true);

            tag.MergeObjectProperties(new { Class = "someotherclass", type = "tagtype" });
            Assert.AreEqual(tag.Render(), "<tagname class = \"someotherclass\" type = \"tagtype\"><tag2></tag2>\r\n</tagname>\r\n");
        }

    }
}
