using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFormsUtilities.Examples.Classes;
using System.Web.UI.MobileControls;

namespace WebFormsUtilities.Examples.Registration {

    public partial class RegistrationHtmlHelper : System.Web.UI.Page, IWebFormsView<Customer> {

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
            Model = Customer.CreateEmptyMockup();

            if (Page.IsPostBack) {     
                //If we are doing a postback, load the values into the model
                //This preserves our state for these controls, since they do not
                //utilize viewstate

                WFPageUtilities.UpdateModel(Request, Model);
                WFPageUtilities.UpdateModel(Request, Model.Login);
                WFPageUtilities.UpdateModel(Request, Model.Address);
            }
        }

        protected void btnSubmit_OnClick(object sender, EventArgs e) {
            //We can pass "this", because this page implements IWebFormsView
            //Run validation rules defined on Customer properties...
            WFPageUtilities.TryValidateModel(this);
            //...and CustomerAddress properties...
            WFPageUtilities.TryValidateModel(this, typeof(CustomerAddress));
            //...and CustomerLogin properties...
            WFPageUtilities.TryValidateModel(this, typeof(CustomerLogin));

            //TryValidateModel returns a bool, but we want to know if any of the validations
            //above succeeded or failed. The Html object stores a WFMetaData object which
            //holds all of the error information. We can check if it has any errors after
            //running validation above.
            if(Html.MetaData.Errors.Count < 0) {
                Response.Redirect("~/Registration/RegistrationThanks.aspx");
            }
        }

    }
}
