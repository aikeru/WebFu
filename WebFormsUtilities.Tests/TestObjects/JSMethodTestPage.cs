using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace WebFormsUtilities.Tests.TestObjects {
    public class JSMethodTestPage : Page {

        public string Output { get; set; }

        public JSMethodTestPage() {
            Output = "";
        }

        [WFJScriptMethod]
        public void TestMethodSafe() {
            //Should execute from CallJSMethod
            Output = "TestMethodSafe";
        }

        public void TestMethodUnsafe() {
            //Should never execute from CallJSMethod
            Output = "TestMethodUnsafe";
        }

    }
}
