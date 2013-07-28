<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PartialPage.aspx.cs" Inherits="WebFormsUtilities.Examples.Partials.PartialPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <script src="http://ajax.aspnetcdn.com/ajax/jquery/jquery-1.4.2.js" type="text/javascript"></script>
    <script src="http://ajax.aspnetcdn.com/ajax/jquery.validate/1.7/jquery.validate.js" type="text/javascript"></script>
    <style type="text/css">
        .input-validation-error {
            border: 1px solid #ff0000;
            background-color: #ffeeee;
        }
        .field-validation-error {
            color: #ff0000;
        }
        .field-validation-valid {
            display: none;
        }
        .validation-summary-errors {
            font-weight: bold;
            color: #ff0000;
        }
        .validation-summary-valid {
            display: none;
        }
        .input-unique-valid {
            border: 1px solid #00ff00;
            background-color: #eeffee;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <%=WebFormsUtilities.WFUtilities.RenderControl("~/Partials/PartialCustomerView.ascx", Customer, MetaData) %>

    </div>
    <%=WebFormsUtilities.WFUtilities.RenderControl("~/Partials/PartialCustomerAddressView.ascx", CustomerAddress, MetaData) %>
    <%=WebFormsUtilities.WFPageUtilities.EnableClientValidation(MetaData) %>
    <input type="submit" value="Submit Form" />
    </form>
   
</body>
</html>
