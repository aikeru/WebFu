using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFormsUtilities.Examples.Classes;

namespace WebFormsUtilities.Examples.Functional {
    public partial class ClientSideValidation : System.Web.UI.Page {

        public Customer Model { get; set; }
        private HtmlHelper<Customer> _Html = null;
        public HtmlHelper<Customer> Html {
            get { if (_Html == null) { _Html = new HtmlHelper<Customer>(this, Model); } return _Html; }
            set { _Html = value; }
        }
        protected void Page_Load(object sender, EventArgs e) {
            //Make sure our Model reference isn't null.
            Model = new Customer();
            if (Page.IsPostBack) {
                //If posting back, be sure to update our model.
                //This also preserves our state between postbacks.
                WFPageUtilities.UpdateModel(Request, Model);
            }
        }
        protected void btnSave_OnClick(object sender, EventArgs e) {
            Model.Save();
            lblState.Text = "Person saved.";
        }
    }
}
