using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFormsUtilities;

namespace WebFormsUtilities.Examples.Helpers {
    public partial class HtmlTagExample : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {

        }
        public string GenerateHTMLFromCS() {
            HtmlTag div = new HtmlTag("div", new { style = "background-color:Blue;" });
            HtmlTag span = new HtmlTag("span") { InnerText = "This HTML was created by HtmlTag.cs" };
            span.Attr("style", "font-style:italic; font-weight: bold;");
            div.Children.Add(span);
            return div.Render();

        }
    }
}
