using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFormsUtilities.Examples.Classes;

namespace WebFormsUtilities.Examples.Partials
{
    public partial class PartialPage : System.Web.UI.Page {

        public Customer Customer { get; set; }
        public CustomerAddress CustomerAddress { get; set; }
        public WFModelMetaData MetaData { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Customer = new Customer();
            CustomerAddress = new CustomerAddress();
            MetaData = new WFModelMetaData();
            if (Page.IsPostBack)
            {
                WFPageUtilities.UpdateModel(Request, Customer);
                WFPageUtilities.UpdateModel(Request, CustomerAddress);
                WFPageUtilities.TryValidateModel(MetaData, typeof(CustomerAddress), HttpContext.Current);
                WFPageUtilities.TryValidateModel(MetaData, typeof(Customer), HttpContext.Current);
            }
        }
    }
}