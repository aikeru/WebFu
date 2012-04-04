using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFormsUtilities.Examples.Classes;
using WebFormsUtilities.ValueProviders;

namespace WebFormsUtilities.Examples.MyAccount {
    public partial class MyAccountServerControlsXML : System.Web.UI.Page, IWebFormsView<Customer> {

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

            State.Items.Add(new ListItem() { Text = "Select State", Value = "" });
            foreach (SelectListItem sli in StateUtility.GetStateList(State.SelectedValue ?? "")) {
                State.Items.Add(new ListItem() { Text = sli.Text, Value = sli.Value });
            }
        }
    }
}
