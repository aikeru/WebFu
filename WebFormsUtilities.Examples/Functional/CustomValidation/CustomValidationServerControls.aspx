<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomValidationServerControls.aspx.cs" Inherits="WebFormsUtilities.Examples.Functional.CustomValidation.CustomValidationServerControls" %>
<%@ Import Namespace = "WebFormsUtilities" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <!-- These are just to make the examples look nice -->
    <script src="../../Scripts/prettify.js" language="javascript" type="text/javascript"></script>
    <link href="../../styles/prettify.css" rel="stylesheet" type="text/css" />
    <!-- These two scripts are required for WebFormsUtilities -->
    <script src="http://ajax.aspnetcdn.com/ajax/jquery/jquery-1.4.2.js" type="text/javascript"></script>
    <script src="../../Scripts/WFUtilitiesJquery.js" type="text/javascript"></script>
    <!-- jQuery validate is required for client-side validation -->
    <script src="http://ajax.aspnetcdn.com/ajax/jquery.validate/1.7/jquery.validate.js" type="text/javascript"></script>
    <!-- Sample error styling -->
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
    <script type="text/javascript">
    //This is an example of a custom validator

    jQuery.validator.addMethod("price", function(value, element, params) {
        if (this.optional(element)) {
            return true;
        }

        if (value > params.min) {
            var cents = value - Math.floor(value);
            if (cents >= 0.99 && cents < 0.995) {
                return true;
            }
        }
        return false;
    });

    //This function forces a full postback even though the form may be invalid.
    function doForce() {
        $('form')[0].submit();
    }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <p>This example demonstrates both client-side and server-side validation using a custom validator in WebFormsUtilities.</p>
        <h1>Here is an example of custom validation in action : </h1>
        <i>(Both server and client-side validation have been enabled)</i>
        <p>After successful postback, a label server control will be updated to show that the model has been saved.</p>
        <!-- This is where the summary of errors would be displayed if server side validation was enabled. -->
        <asp:ValidationSummary ID="valSummary1" runat="server" />
    
        <!-- LabelFor is optional, and creates an HTML label for this field. -->
        <!-- It can also be used for globalization or to easily make changes to the name of a field across a project. -->
        <WebFu:LabelFor ID="FirstNameLabel" runat="server" PropertyName="FirstName" TargetControl="FirstName" /> :
        <!-- Generate a TextBox for the FirstName property with validation enabled -->
        <asp:TextBox ID="FirstName" runat="server" />
        <!-- ValidationMessageFor is optional, since a ValidationSummary can be used -->
        <WebFu:DataAnnotationValidatorControl ID="DataAnnotationValidatorControl1" 
                                              runat="server" 
                                              ControlToValidate="FirstName"
                                              PropertyName="FirstName" />
        <br />
        <WebFu:LabelFor ID="LastNameLabel" runat="server" PropertyName="LastName" TargetControl="LastName" /> :
        <asp:TextBox ID="LastName" runat="server" />
        <WebFu:DataAnnotationValidatorControl ID="DataAnnotationValidatorControl2" 
                                              runat="server" 
                                              ControlToValidate="LastName"
                                              PropertyName="LastName" />
        <br />
        <WebFu:LabelFor ID="PriceLabel" runat="server" PropertyName="Price" TargetControl="Price" /> :
        <asp:TextBox ID="Price" runat="server" />
        <WebFu:DataAnnotationValidatorControl ID="DataAnnotationValidatorControl3" 
                                              runat="server" 
                                              ControlToValidate="Price"
                                              PropertyName="Price" />
        <br />
        
        <!-- WebFormsUtilities plays nice with server controls -->
        <asp:Button ID="btnSave" runat="server" Text="Save (trigger client validation)" OnClick="btnSave_OnClick" /><br />
        <asp:Label ID="lblState" runat="server" Text="Initial State"></asp:Label>
        <br />
        <!-- Force a full postback, regardless of validation. Return false; so submission can't happen 2x -->
        <button onclick="doForce(); return false;">Force Postback (bypass client validation)</button>
    </div>
    <!-- This is required for client-side validation. Run this at the end of the form -->
    <WebFu:EnableClientValidationControl ID="EnableClientValidationControl1" runat="server" />
    </form>
</body>
</html>
