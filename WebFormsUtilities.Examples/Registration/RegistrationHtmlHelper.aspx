<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegistrationHtmlHelper.aspx.cs" Inherits="WebFormsUtilities.Examples.Registration.RegistrationHtmlHelper" %>
<%@ Import Namespace="WebFormsUtilities.Examples.Classes" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <script src="http://ajax.aspnetcdn.com/ajax/jquery/jquery-1.4.2.js" type="text/javascript"></script>
    <script src="http://ajax.aspnetcdn.com/ajax/jquery.validate/1.7/jquery.validate.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <%=Html.LabelFor(model => model.FirstName) %> :
        <%=Html.TextBoxFor(model => model.FirstName) %>
        <%=Html.ValidationMessageFor(model => model.FirstName) %>
        <br />
        <%=Html.LabelFor(model => model.LastName) %> :
        <%=Html.TextBoxFor(model => model.LastName) %>
        <%=Html.ValidationMessageFor(model => model.LastName) %>
        <br />
        <%=Html.LabelFor(model => model.Email) %> :
        <%=Html.TextBoxFor(model => model.Email) %>
        <%=Html.ValidationMessageFor(model => model.Email) %>
        <br />
        <%=Html.LabelFor(model => model.SocialSecurityNumber) %> :
        <%=Html.TextBoxFor(model => model.SocialSecurityNumber)%>
        <%=Html.ValidationMessageFor(model => model.SocialSecurityNumber)%>
        <br />
        <%=Html.LabelFor(model => model.Address.Address1) %> :
        <%=Html.TextBoxFor(model => model.Address.Address1) %>
        <%=Html.ValidationMessageFor(model => model.Address.Address1) %>
        <br />
        <%=Html.LabelFor(model => model.Address.Address2) %> :
        <%=Html.TextBoxFor(model => model.Address.Address2) %>
        <%=Html.ValidationMessageFor(model => model.Address.Address2) %>
        <br />
        <%=Html.LabelFor(model => model.Address.City) %> :
        <%=Html.TextBoxFor(model => model.Address.City) %>
        <%=Html.ValidationMessageFor(model => model.Address.City) %>
        <br />
        <%=Html.LabelFor(model => model.Address.State) %> :
        <%=Html.DropDownListFor(model => model.Address.State, StateUtility.GetStateList(Model.Address.State), "Select State") %>
        <%=Html.ValidationMessageFor(model => model.Address.State) %>
        <br />
        <%=Html.LabelFor(model => model.Login.UserName) %> :
        <%=Html.TextBoxFor(model => model.Login.UserName) %>
        <%=Html.ValidationMessageFor(model => model.Login.UserName) %>
        <br />
        <%=Html.LabelFor(model => model.Login.Password) %> :
        <%=Html.TextBoxFor(model => model.Login.Password, new { type = "password" })%>
        <%=Html.ValidationMessageFor(model => model.Login.Password) %>
        <br />
        <%=Html.LabelFor(model => model.Login.ConfirmPassword) %> :
        <%=Html.TextBoxFor(model => model.Login.ConfirmPassword)%>
        <%=Html.ValidationMessageFor(model => model.Login.ConfirmPassword)%>
        <br />
        <!-- WebFormsUtilities plays nice with server controls -->
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_OnClick" /><br />
        <asp:Label ID="lblState" runat="server" Text="Initial State"></asp:Label>
        <br />
    </div>
    <!-- This is required for client-side validation. Run this at the end of the form -->
    <%=WebFormsUtilities.WFPageUtilities.EnableClientValidation(Html.MetaData) %>
    </form>
</body>
</html>
