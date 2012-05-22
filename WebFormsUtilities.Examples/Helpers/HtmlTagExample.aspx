<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HtmlTagExample.aspx.cs" Inherits="WebFormsUtilities.Examples.Helpers.HtmlTagExample" %>
<%@ Import Namespace="WebFormsUtilities" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <style>
        .clsDivBox 
        {
            width: 200px;
            height: 200px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        HtmlTag.cs and WFHtmlTag.js are libraries that assist in generating HTML content.<br />
        Markup generated from code-behind can be much more maintainable using this method and manipulated in an object-oriented manner.<br />
        <script src="../Scripts/WFHtmlTag.js"></script>
        <script type="text/javascript" language="javascript">
            //Illustrate generating HTML from JavaScript using WFHtmlTag.js

            function GenerateHTMLFromJS() {
            
                var div = webfu.HtmlTag("div", { "style": "background-color:Red;" });
                var span = webfu.HtmlTag("span")({ InnerText: "This HTML was created by WFHtmlTag.js" });
                span.Attr("style", "font-style:italic; font-weight: bold;");
                div.Children.Add(span);
                div.AddClass("clsDivBox");
                return div.Render();
                
            }

            document.write(GenerateHTMLFromJS());
        </script>
        <script runat="server" language="C#">
            //Illustrate generating HTML from C# using HtmlTag.js
            
            string GenerateHTMLFromCS() {
                
                HtmlTag div = new HtmlTag("div", new { style = "background-color:Blue;" });
                HtmlTag span = new HtmlTag("span") { InnerText = "This HTML was created by HtmlTag.cs" };
                span.Attr("style", "font-style:italic; font-weight: bold;");
                div.Children.Add(span);
                div.AddClass("clsDivBox");
                return div.Render();
                
            }
        </script>
        <%=GenerateHTMLFromCS()%>
    </div>
    </form>
</body>
</html>
