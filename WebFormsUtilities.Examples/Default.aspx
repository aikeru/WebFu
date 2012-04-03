<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebFormsUtilities.Examples._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.5/jquery.min.js"></script>
    <script src="/scripts/grid.locale-en.js" type="text/javascript"></script>
    <script src="/scripts/jquery.jqGrid.min.js" type="text/javascript"></script>
    <script src="/scripts/json.min.js" type="text/javascript"></script>
    <script src="/scripts/WFUtilitiesJquery.js"></script>
        <link href="/styles/normalize.css" rel="stylesheet" type="text/css" />
    <link href="/styles/site.css" rel="stylesheet" type="text/css" />
    <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css" rel="stylesheet" type="text/css"/>
    <link href="/styles/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="/styles/jquery.ui.tabs.css" rel="stylesheet" type="text/css" />
    <link href="/styles/ui.jqgrid.css" rel="stylesheet" type="text/css" />
    <script>
        //public static string TestAJAX(string argStr, int argInt, int[] argIntArr, string[] argStrArr) {
        function doAJAX() {
            $wf.callPage("Default.aspx/TestAJAX", {
                data: { argStr: "awesome", argInt: 1337, argIntArr: [1, 3, 3, 7], argStrArr: ["awe", "some"] },
                success: function(msg) {
                    alert(msg.d);
                },
                error: function() {
                    alert('ajax error');
                }
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    </div>
    <button onclick="doAJAX(); return false;"></button>
    </form>
</body>
</html>
