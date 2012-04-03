using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFormsUtilities.Examples.Classes;
using WebFormsUtilities.DataAnnotations;
using WebFormsUtilities.ValueProviders;

namespace WebFormsUtilities.Examples.Functional.CustomValidation {

    // There are several options to specify validation rules for DataAnnotationValidator controls
    //   - In XML, each control must point to an XmlRuleSet
    //   - In DataAnnotation's on the model per control (each control must set SourceTypeString)
    //   - At the page level by implementing IWFGetValidationRulesForPage
    // You can mix and match if necessary.

    public partial class CustomValidationServerControls : System.Web.UI.Page, IWFGetValidationRulesForPage {
        public Customer Model { get; set; }
        public bool ModelIsValid = false;

        protected void Page_Load(object sender, EventArgs e) {
            //Make sure our Model reference isn't null.
            Model = new Customer();
            if (Page.IsPostBack) {
                //If posting back, be sure to update our model from the page controls.
                WFPageUtilities.UpdateModel(new WFPageControlsValueProvider(this, ""),
                                            Model, "", null, null);
            }
        }
        protected void btnSave_OnClick(object sender, EventArgs e) {
            //WebFu:DataAnnotationValidator controls will fire automatically on postback.
            //If the form validated...
            if (Page.IsValid) {
                //We already populated the model in Page_Load (line 21), so save it.
                Model.Save(); 
                lblState.Text = "Person saved.";
            } else {
                lblState.Text = "Failed validation.";
            }
        }

        #region IWFGetValidationRulesForPage Members
        
        public Type GetValidationClassType() {
            return typeof(Customer);
        }

        #endregion
    }
}
