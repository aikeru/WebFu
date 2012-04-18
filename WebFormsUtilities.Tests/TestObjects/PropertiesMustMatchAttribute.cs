using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using WebFormsUtilities.DataAnnotations;

namespace WebFormsUtilities.Tests.TestObjects {
    public class PropertiesMustMatchAttribute : ValidationAttribute, IWFClientValidatable {
        public String FirstPropertyName { get; set; }
        public String SecondPropertyName { get; set; }

        public PropertiesMustMatchAttribute() {
        }

        public PropertiesMustMatchAttribute(String firstPropertyName, string secondPropertyName) {
            FirstPropertyName = firstPropertyName;
            SecondPropertyName = secondPropertyName;
        }
        public override bool IsValid(object value) {
            Type objectType = value.GetType();
            PropertyInfo[] props = objectType.GetProperties().Where(p => p.Name == FirstPropertyName ||
                p.Name == SecondPropertyName).ToArray();
            if (props.Count() != 2) {
                ErrorMessage = "Invalid property names or could not find properties on object " + objectType.Name;
                return false;
            }
            if (props[0].GetValue(value, null).ToString()
                .Equals(props[1].GetValue(value, null).ToString())) {
                return true;
            }
            //Could derive from displaynameattribute, too
            if (String.IsNullOrEmpty(ErrorMessage)) {
                ErrorMessage = props[0].Name + " and " + props[1].Name + " don't match.";
            }

            return false;
        }

        #region IWFClientValidatable Members

        public IEnumerable<WFModelClientValidationRule> GetClientValidationRules() {
            var rule = new WFModelClientValidationRule {
                ErrorMessage = ErrorMessage,
                ValidationType = "propsmatch"
            };
            rule.ValidationParameters.Add("firstProp", FirstPropertyName);
            rule.ValidationParameters.Add("secondProp", SecondPropertyName);
            return new[] { rule };
        }

        public WFModelMetaProperty MetaProperty {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
