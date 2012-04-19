using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

namespace WebFormsUtilities.Examples.ExposeWebMethods {
    public partial class ExposeWebMethodsExample : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            if (Page.IsPostBack) {
                WFPageUtilities.CallJSMethod(this, Request);
            }
        }

        //Methods that are not decorated with WFJScriptMethod cannot be invoked by WFPageUtilities.CallJSMethod()
        //This ensures that it is limited to only methods intended to be invoked in this manner.
        [WFJScriptMethod]
        protected void DoWorkOnPostBack(object sender, EventArgs e) {
            lblWork.Text = "Did Some Work Via Postback at " + DateTime.Now.ToString();
        }

        [WebMethod]
        public static string DoSomeAJAXWork(string propertyOne, int[] propertyTwo) {
            return "Did Some Work Via AJAX!";
        }
    }
}
