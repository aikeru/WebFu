using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFormsUtilities.Examples.Classes;
using System.Text.RegularExpressions;
using WebFormsUtilities.Examples.Functional.CustomValidation;

namespace WebFormsUtilities.Examples.Registration {
    public partial class RegistrationWebForms : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            State.Items.Add(new ListItem() { Text = "Select State", Value = "" });
            foreach (SelectListItem sli in StateUtility.GetStateList(State.SelectedValue ?? "")) {
                State.Items.Add(new ListItem() { Text = sli.Text, Value = sli.Value });
            }
        }


        public string EmailRegexPattern {
            get { return ValidEmailAttribute.RegexPattern; }
        }

        protected void OnServerValidate_PasswordMatch(object sender, EventArgs e) {
            if(Password.Text == ConfirmPassword.Text) {
                ((ServerValidateEventArgs)e).IsValid = true;
            }
        }
        protected void OnServerValidate_Email(object sender, EventArgs e) {
            ((ServerValidateEventArgs)e).IsValid = Regex.IsMatch(((ServerValidateEventArgs)e).Value, EmailRegexPattern);
        }
        //See markup comments for why there are two StringLength validators here...
        protected void OnServerValidate_StringLength_FirstName(object sender, EventArgs e) {
            ServerValidateEventArgs validate = ((ServerValidateEventArgs)e);
            if (!String.IsNullOrEmpty(validate.Value)) {
                validate.IsValid = validate.Value.Length > 20;
            } else {
                validate.IsValid = true;
            }
        }
        //See markup comments for why there are two StringLength validators here...
        protected void OnServerValidate_StringLength_LastName(object sender, EventArgs e) {
            ServerValidateEventArgs validate = ((ServerValidateEventArgs)e);
            if (!String.IsNullOrEmpty(validate.Value)) {
                validate.IsValid = validate.Value.Length > 20;
            } else {
                validate.IsValid = true;
            }
        }

        protected void btnSubmit_OnClick(object sender, EventArgs e) {
            if (Page.IsValid) {
                Customer c = Customer.CreateEmptyMockup();
                c.FirstName = FirstName.Text;
                c.LastName = LastName.Text;
                c.Email = Email.Text;
                c.Login.UserName = UserName.Text;
                c.Login.Password = Password.Text;
                c.Address.Address1 = Address1.Text;
                c.Address.Address2 = Address2.Text;
                c.Address.City = City.Text;
                c.Address.State = State.Text;
                c.Save();

                Response.Redirect("~/Registration/RegistrationThanks.aspx");
            }
        }
    }
}
