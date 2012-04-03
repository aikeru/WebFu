using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using WebFormsUtilities.RuleProviders;
using WebFormsUtilities.ValueProviders;

namespace WebFormsUtilities.Examples {
    public class jqGridTestData {
        public string FirstName { get; set; }
        public int ID { get; set; }
    }
    public partial class _Default : System.Web.UI.Page {

        protected void Page_Load(object sender, EventArgs e) {
            
            //List<jqGridTestData> data = new List<jqGridTestData>();
            //data.Add(new jqGridTestData() { FirstName = "Mike", ID = 1 });
            //data.Add(new jqGridTestData() { FirstName = "Andy", ID = 2 });
            //data.Add(new jqGridTestData() { FirstName = "John", ID = 3 });
            //data.Add(new jqGridTestData() { FirstName = "Wolf", ID = 4 });

            //jqGrid.DataSource = data;
            //jqGrid.DataBind();
            //WFTypeRuleProvider ruleProvider = new WFTypeRuleProvider(modelObject);
            //WFObjectValueProvider valueProvider = new WFObjectValueProvider(modelObject, "");
            //WFUtilities.TryValidateModel(ModelMetaData, "", null, null, 
        }
        [WebMethod]
        public static string TestAJAX(string argStr, int argInt, int[] argIntArr, string[] argStrArr) {
            return "success";
        }
    }

    
}
