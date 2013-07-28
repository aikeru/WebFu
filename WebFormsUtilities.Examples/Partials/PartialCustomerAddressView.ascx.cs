using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFormsUtilities.Examples.Classes;

namespace WebFormsUtilities.Examples.Partials
{
    public partial class PartialCustomerAddressView : System.Web.UI.UserControl, IWebFormsView<CustomerAddress> {

        private CustomerAddress _Model = null;

        #region IWebFormsView<CustomerAddress> Members

        public CustomerAddress Model
        {
            get {
                return _Model;
            }
            set {
                _Model = value;
            }
        }

        private HtmlHelper<CustomerAddress> _Html = null;

        public HtmlHelper<CustomerAddress> Html
        {
            get {
                if (_Html == null) {
                    _Html = new HtmlHelper<CustomerAddress>(this);
                }
                return _Html;
            }
        }

        #endregion
    
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}