using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFormsUtilities.Examples.Classes;

namespace WebFormsUtilities.Examples.Functional.CustomValidation {
    public partial class CustomValidationHtmlHelper : System.Web.UI.Page {
        public Customer Model { get; set; }
        public bool ModelIsValid = false;
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
                WFPageUtilities.UpdateModel<Customer>(Request, Model);
                //Show any errors on the form
                ModelIsValid = WFPageUtilities.TryValidateModel(Html.MetaData, Model, HttpContext.Current);

                lblState.Text = "Postback occurred and model failed validation.";
            }
        }
        protected void btnSave_OnClick(object sender, EventArgs e) {
            //This will populate the MetaData with any error information, and Html.TextBoxFor() 
            if (ModelIsValid) {
                Model.Save();
                lblState.Text = "Person saved.";
            } else {
                lblState.Text = "Failed validation.";
            }
        }
    }
}
