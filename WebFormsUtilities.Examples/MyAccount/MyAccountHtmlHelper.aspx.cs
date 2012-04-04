using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFormsUtilities.Examples.Classes;
using WebFormsUtilities.ValueProviders;

namespace WebFormsUtilities.Examples.MyAccount {
    public partial class MyAccountHtmlHelper : System.Web.UI.Page, IWebFormsView<Customer> {

        private Customer _Model = null;

        #region IWebFormsView<Customer> Members

        public Customer Model {
            get {
                return _Model;
            }
            set {
                _Model = value;
            }
        }

        private HtmlHelper<Customer> _Html = null;

        public HtmlHelper<Customer> Html {
            get {
                if (_Html == null) {
                    _Html = new HtmlHelper<Customer>(this);
                }
                return _Html;
            }
        }

        #endregion
        
        protected void Page_Load(object sender, EventArgs e) {
            //If we are loading the page for the first time...
            if (!Page.IsPostBack) {
                //Populate the model. ie: Here is where we would load the model from the database
                //such as from LINQ, ADO, Entity Framework, etc...
                Model = Customer.LoadMockup();
                //That's it! The HtmlHelper will take care of hooking up the values to the UI.
            } else {
                //If we are doing a postback, load the values into the model
                //This preserves our state for these controls, since they do not
                //utilize viewstate

                WFPageUtilities.UpdateModel(Request, Model);
                WFPageUtilities.UpdateModel(Request, Model.Login);
                WFPageUtilities.UpdateModel(Request, Model.Address);
            }
        }

        protected void btnSave_OnClick(object sender, EventArgs e) {
            //Save the model to the database
            //It has already been updated in Page_Load
            Model.Save();

            Response.Redirect("~/Default.aspx");
        }

        protected void btnCancel_OnClick(object sender, EventArgs e) {
            Response.Redirect("~/Default.aspx");
        }
    }
}
