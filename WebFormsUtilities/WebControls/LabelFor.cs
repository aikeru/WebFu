﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Reflection;

namespace WebFormsUtilities.WebControls {
    public class LabelFor : WFWebControlForBase {

        protected override void RenderContents(System.Web.UI.HtmlTextWriter writer) {
            //base.RenderContents(writer);
            string dispName = "";
            PropertyInfo pi = GetTargetProperty();
            
            if (pi == null) { throw new Exception("[" + (PropertyName ?? "null") + "] public property not found on object [" + (GetTypeForControl().Name) + "]"); }
            
            HtmlTag lbl = new HtmlTag("label", 
                new { For = GetTargetControl().ClientID }) { InnerText = DisplayName };
            
            lbl.MergeObjectProperties(this.Attributes);

            writer.Write(lbl.Render());
        }
    }
}
