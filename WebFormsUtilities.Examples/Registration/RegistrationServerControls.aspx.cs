using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFormsUtilities.Examples.Classes;
using WebFormsUtilities.DataAnnotations;
using WebFormsUtilities.ValueProviders;

namespace WebFormsUtilities.Examples.Registration {
    public partial class RegistrationServerControls : Page,
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

        #endregion


        protected void Page_Load(object sender, EventArgs e) {
            Model = Customer.CreateEmptyMockup();

            State.Items.Add(new ListItem() { Text = "Select State", Value = "" });
            foreach (SelectListItem sli in StateUtility.GetStateList(State.SelectedValue ?? "")) {
                State.Items.Add(new ListItem() { Text = sli.Text, Value = sli.Value });
            }
        }

        protected void btnSubmit_OnClick(object sender, EventArgs e) {
            //The WebFu:DataAnnotationValidatorControl uses built-in Web Forms
            //validation and will fire automatically.
            if (Page.IsValid) {
                //If validation succeeded, update the model from the controls on the page:
                WFPageControlsValueProvider provider = new WFPageControlsValueProvider(this, "");
                
                WFPageUtilities.UpdateModel(provider, Model, "", null, null);
                WFPageUtilities.UpdateModel(provider, Model.Login, "", null, null);
                WFPageUtilities.UpdateModel(provider, Model.Address, "", null, null);

                Model.Save();

                Response.Redirect("~/Registration/RegistrationThanks.aspx");
            }
            
        }


        #region IWFGetValidationRulesForPage Members

        public Type GetValidationClassType() {
            return typeof(Customer);
        }

        #endregion
    }
}
