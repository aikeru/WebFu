using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFormsUtilities.DataAnnotations;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace WebFormsUtilities.WebControls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:EnableClientValidationControl runat=server></{0}:EnableClientValidationControl>")]
    public class EnableClientValidationControl : WebControl
    {
        protected override void RenderContents(HtmlTextWriter output)
        {

            WFModelMetaData metadata = new WFModelMetaData();
            foreach (DataAnnotationValidatorControl dvc in WebControlUtilities.FindValidators(this.Page))
            {
                WFModelMetaProperty metaprop = WebControlUtilities.GetMetaPropertyFromValidator(this.Page, dvc, metadata);
                metaprop.OverriddenSpanID = dvc.UniqueID;
                if (!String.IsNullOrEmpty(dvc.Text))
                {
                    metaprop.OverriddenErrorMessage = dvc.Text;
                }
                metadata.Properties.Add(metaprop);
            }

            output.Write(new HtmlTag("script", new { type = "text/javascript", language = "javascript" }) { InnerText = WFUtilities.EnableClientValidationScript(metadata) }.Render());
        }
    }
}
