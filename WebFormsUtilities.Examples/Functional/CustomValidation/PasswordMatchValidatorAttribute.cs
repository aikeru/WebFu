using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFormsUtilities.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace WebFormsUtilities.Examples.Functional.CustomValidation {
    public class PasswordMustMatchAttribute : ValidationAttribute, IWFClientValidatable, IWFRequireValueProviderContext {

        IWFValueProvider _ValueProvider = null;

        public override bool IsValid(object value) {
            if (_ValueProvider == null) { throw new Exception("No value provider for context."); }
            string password = (_ValueProvider.KeyValue("Password") ?? new Object()).ToString();
            string confirm = (_ValueProvider.KeyValue("ConfirmPassword") ?? new Object()).ToString();
            if (password == confirm) {
                return true;
            }
            return false;
        }

        #region IWFClientValidatable Members

        public IEnumerable<WFModelClientValidationRule> GetClientValidationRules() {
            var rule = new WFModelClientValidationRule {
                ErrorMessage = ErrorMessage,
                ValidationType = "propsmatch"
            };
            rule.ValidationParameters.Add("firstProp", "Password");
            rule.ValidationParameters.Add("secondProp", "ConfirmPassword");
            return new[] { rule };
        }

        public WebFormsUtilities.WFModelMetaProperty MetaProperty {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region IWFRequireValueProviderContext Members

        public void SetValueProvider(IWFValueProvider provider) {
            _ValueProvider = provider;
        }

        #endregion
    }
}
