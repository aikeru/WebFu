using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFormsUtilities.Examples.Classes;

namespace WebFormsUtilities.Examples.Partials
{
    public partial class PartialView : System.Web.UI.UserControl, IWebFormsView<Customer> {

        private Customer _Model = null;

        #region IWebFormsView<Customer> Members

        public Customer Model {
            get {
                return _Model;
            }
            set {
                _Model = value;
            }
        }

        private HtmlHelper<Customer> _Html = null;

        public HtmlHelper<Customer> Html {
            get {
                if (_Html == null) {
                    _Html = new HtmlHelper<Customer>(this);
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