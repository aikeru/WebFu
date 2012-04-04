using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFormsUtilities.Examples.Classes;
using WebFormsUtilities.ValueProviders;
using WebFormsUtilities.DataAnnotations;

namespace WebFormsUtilities.Examples.MyAccount {
    public partial class MyAccountServerControls : System.Web.UI.Page, IWebFormsView<Customer>,
            IWFGetValidationRulesForPage {

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
            State.Items.Add(new ListItem() { Text = "Select State", Value = "" });
            foreach (SelectListItem sli in StateUtility.GetStateList(State.SelectedValue ?? "")) {
                State.Items.Add(new ListItem() { Text = sli.Text, Value = sli.Value });
            }

            //If we are loading the page for the first time ...
            if (!Page.IsPostBack) {
                //Load a sample customer
                Model = Customer.LoadMockup();
                //This ApplyModelToPage call will create an IWFValueProvider from the model object
                //..and apply the values from this instance to the page controls.
                WebControlUtilities.ApplyModelToPage(this, Model);
                WebControlUtilities.ApplyModelToPage(this, Model.Address);
                WebControlUtilities.ApplyModelToPage(this, Model.Login);
            }

        }

        protected void btnSave_OnClick(object sender, EventArgs e) {
            //Create a value provider from this page. 
            //Values will be derived from controls on this page that match property names on the model.
            IWFValueProvider provider = new WFPageControlsValueProvider(this, "");
            //Update the model from the provider:
            WFPageUtilities.UpdateModel(provider, Model, "", null, null);

            Model.Save();

            Response.Redirect("~/Default.aspx");
        }

        protected void btnCancel_OnClick(object sender, EventArgs e) {
            Response.Redirect("~/Default.aspx");
        }

        #region IWFGetValidationRulesForPage Members

        public Type GetValidationClassType() {
            return typeof(Customer);
        }

        #endregion
    }
}
