using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using WebFormsUtilities.DataAnnotations;

namespace WebFormsUtilities.Tests.TestObjects {
    public class PriceAttribute : ValidationAttribute, IWFClientValidatable {

        public double MinPrice { get; set; }
        public override bool IsValid(object value) {
            if (value == null) { return true; } //Not required
            if (String.IsNullOrEmpty(value.ToString())) { return true; }
            var price = Double.Parse(value.ToString());
            if (price < MinPrice) { return false; }
            double cents = price - Math.Truncate(price);
            if (cents < 0.99 || cents >= 0.995) {
                return false;
            }
            return true;
        }

        #region IWFClientValidatable Members

        public IEnumerable<WFModelClientValidationRule> GetClientValidationRules() {
            var rule = new WFModelClientValidationRule {
                ErrorMessage = ErrorMessage,
                ValidationType = "price"
            };
            rule.ValidationParameters.Add("min", MinPrice);

            return new[] { rule };
        }

        private WFModelMetaProperty _MetaProperty = null;

        public WFModelMetaProperty MetaProperty {
            get {
                return _MetaProperty;
            }
            set {
                _MetaProperty = value;
            }
        }

        #endregion
    }
}
 