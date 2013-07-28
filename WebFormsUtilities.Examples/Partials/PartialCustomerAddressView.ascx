<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PartialCustomerAddressView.ascx.cs" Inherits="WebFormsUtilities.Examples.Partials.PartialCustomerAddressView" %>
Address 1 : <%=Html.TextBoxFor(m => m.Address1) %>
<%=Html.ValidationMessageFor(m => m.Address1) %><br />