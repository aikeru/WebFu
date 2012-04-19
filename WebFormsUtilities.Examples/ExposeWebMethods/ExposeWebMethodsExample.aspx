<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExposeWebMethodsExample.aspx.cs" Inherits="WebFormsUtilities.Examples.ExposeWebMethods.ExposeWebMethodsExample" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <script src="http://ajax.aspnetcdn.com/ajax/jquery/jquery-1.4.2.js" type="text/javascript"></script>
    <script src="/scripts/WFUtilitiesJquery.js"></script>
    <script>
        function doAJAXWork() {
            alert('server returned ' + webfu.page.DoSomeAJAXWork("a string", [1, 2, 3]).d);
        }
        function doAJAXAsyncWork() {
            webfu.page.DoSomeAJAXWorkAsync(function(returnText) {
                alert('server returned async ' + returnText.d);
            },
            function() {
                alert('oh no, there was an error.');
            },
            "a string", [1, 2, 3]);
        }
        function doPostBackWork() {
            webfu.page.DoWorkOnPostBack();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <button onclick="doAJAXWork(); return false;">Do some AJAX work</button>
    <button onclick="doAJAXAsyncWork(); return false;">Do some async AJAX work</button>
    <button onclick="doPostBackWork(); return false;">Do some postback work</button>
    <div>
        <asp:Label ID="lblWork" runat="server" Text=" " />
        <WebFu:ExposeWebMethodsControl runat="server" />
    </div>
    </form>
</body>
</html>
