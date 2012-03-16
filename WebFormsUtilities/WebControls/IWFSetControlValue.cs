using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebFormsUtilities.WebControls {
    /// <summary>
    /// Server controls that wish to use ApplyModelToPage() can implement this interface.
    /// </summary>
    interface IWFSetControlValue {
        void SetControlValue(object value);
    }
}
