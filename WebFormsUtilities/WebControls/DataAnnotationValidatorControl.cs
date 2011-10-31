using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.Web.UI;
using System.ComponentModel;

namespace WebFormsUtilities.WebControls
{
    public class DataAnnotationValidatorControl : BaseValidator
    {
        private string _propertyName = "";
        public string PropertyName
        {
            get
            { return _propertyName; }
            set { _propertyName = value; }
        }
        private string _sourceTypeString = "";
        public string SourceTypeString
        {
            get { return _sourceTypeString; }
            set { _sourceTypeString = value; }
        }
        private Type _sourceType = null;
        public Type SourceType
        {
            get
            { return _sourceType; }
            set { _sourceType = value; }
        }

        private bool _HasBeenChecked = false;

        protected override bool EvaluateIsValid()
        {
            _HasBeenChecked = true;
            if (String.IsNullOrEmpty(SourceTypeString))
            {
                if (SourceType == null)
                { throw new Exception("The SourceType and SourceTypeString properties are null/empty on one of the validator controls.\r\nPopulate either property.\r\nie: control.SourceType = typeof(Widget); OR in markup SourceTypeString=\"Assembly.Classes.Widget, Assembly\""); }
            }
            else
            {
                try
                {

                    SourceType = Type.GetType(SourceTypeString, true, true);
                }
                catch (Exception ex)
                {
                    throw new Exception("Couldn't resolve type " + SourceTypeString + ". You may need to specify the fully qualified assembly name.");
                }
            }
            PropertyInfo prop = SourceType.GetProperty(_propertyName);
            Control validateControl = this.FindControl(this.ControlToValidate); //Search siblings
            if(validateControl == null) //Search page
            {
                validateControl = WebControlUtilities.FindControlRecursive(this.Page, this.ControlToValidate);
            }
            string controlValue = WebControlUtilities.GetControlValue(validateControl);

            foreach(var attr in prop.GetCustomAttributes(typeof(ValidationAttribute), true).OfType<ValidationAttribute>())
            {
                var displayNameAttr = prop.GetCustomAttributes(typeof(DisplayNameAttribute), true).OfType<DisplayNameAttribute>().FirstOrDefault();
                string displayName = displayNameAttr == null ? prop.Name : displayNameAttr.DisplayName;
                
                if (!attr.IsValid(controlValue))
                {
                    this.ErrorMessage = attr.FormatErrorMessage(displayName);
                    return false;
                }
            }
            return true;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            //Base functionality will make this hidden, causing it not to work for client validation.
            //Override here, rendering the same thing, just without display:none;
            HtmlTag spn = new HtmlTag("span", new
            {
                id = this.UniqueID,
                style = "color:Red;"
            });
            //Allow server if the validator has been checked before
            if (_HasBeenChecked && !EvaluateIsValid())
            {
                //Use Text="" attribute if specified
                if (!String.IsNullOrEmpty(this.Text))
                {
                    spn.InnerText = this.Text;
                }
                else //Otherwise, use the nice formatted error message from DataAnnotations
                {
                    spn.InnerText = this.ErrorMessage;
                }
            }

            //base.Render(writer);
            writer.Write(spn.Render());
        }
    }
}
