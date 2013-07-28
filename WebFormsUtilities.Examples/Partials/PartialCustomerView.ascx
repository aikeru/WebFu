<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PartialCustomerView.ascx.cs" Inherits="WebFormsUtilities.Examples.Partials.PartialView" %>
FirstName : <%=Html.TextBoxFor(m => m.FirstName) %>
<%=Html.ValidationMessageFor(m => m.FirstName) %>
<br />