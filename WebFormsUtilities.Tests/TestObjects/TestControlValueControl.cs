using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using WebFormsUtilities.WebControls;

namespace WebFormsUtilities.Tests.TestObjects {
    class TestControlValueControl : WebControl, IWFControlValue {

        public string ControlValue { get; set; }

        #region IWFControlValue Members

        public void SetControlValue(object value) {
            ControlValue = (value ?? "").ToString();
        }

        public object GetControlValue() {
            return ControlValue ?? "";
        }

        #endregion
    }
}
