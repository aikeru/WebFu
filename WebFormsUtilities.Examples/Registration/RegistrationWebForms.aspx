<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegistrationWebForms.aspx.cs" Inherits="WebFormsUtilities.Examples.Registration.RegistrationWebForms" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
        <!-- 
        
            ****  This example shows what the "Registration" demo might look like ****
            ****  using "plain-vanilla" ASP.NET Web Forms                         ****
            ****  Nothing on this page uses any 3rd party library.                **** -->
    <title></title>
    <script>
        //Why two JavaScript validators for FirstName and LastName string length?
        //This is to represent that since you can't specify the minimum/maximum with
        //the custom validator, you would have to have a separate validator for each
        //differing length.
        function ValidateStringLength_FirstName(source, args) {
            args.IsValid = (args.Value.length <= 10);
        }
        function ValidateStringLength_LastName(source, args) {
            args.IsValid = (args.Value.length <= 10);
        }
        //Why not just use a "regular expression validator" here?
        //The other examples (WebFu) use a custom validator for e-mail,
        //so this keeps it consistent.
        function ValidateEmail(source, args) {
            var expr = /<%=EmailRegexPattern %>/;
            if (expr.test(args.Value)) {
                args.IsValid = true;
            } else {
                args.IsValid = false;
            }
        }
        //Match Password and ConfirmPassword
        function Validate_PasswordMatch(source, args) {
            if($get("<%=Password.ClientID %>").value === args.Value) {
                args.IsValid = true;
            }
            args.IsValid = false;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="FirstNameLabel" runat="server" Text="First Name" /> :
        <asp:TextBox ID="FirstName" runat="server" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                                    runat="server"
                                    ControlToValidate="FirstName"
                                    EnableClientScript="true">First Name is Required</asp:RequiredFieldValidator>
        <asp:CustomValidator ID="CustomValidator1"
                             runat="server"
                             ControlToValidate="FirstName"
                             EnableClientScript="true"
                             ClientValidationFunction="ValidateStringLength_FirstName"
                             OnServerValidate="OnServerValidate_StringLength_FirstName">First Name must be 20 characters or less.</asp:CustomValidator>
        <br />
        <asp:Label ID="LastNameLabel" runat="server" Text="Last Name" /> :
        <asp:TextBox ID="LastName" runat="server" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2"
                                    runat="server"
                                    ControlToValidate="LastName"
                                    EnableClientScript="true">Last Name is Required.</asp:RequiredFieldValidator>
        <asp:CustomValidator ID="CustomValidator2"
                             runat="server"
                             ControlToValidate="LastName"
                             EnableClientScript="true"
                             ClientValidationFunction="ValidateStringLength_LastName"
                             OnServerValidate="OnServerValidate_StringLength_LastName">Last Name must be 20 characters or less.</asp:CustomValidator> 
         <br />
         <asp:Label ID="SocialSecurityNumberLabel" runat="server" Text="Social Security Number" /> :
         <asp:TextBox ID="SocialSecurityNumber" runat="server" />
         <asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                                         runat="server"
                                         ControlToValidate="SocialSecurityNumber"
                                         EnableClientScript="true"
                                         ValidationExpression="^((?!000)([0-6]\d{2}|[0-7]{2}[0-2]))-((?!00)\d{2})-((?!0000)\d{4})$">Must be a valid SSN.</asp:RegularExpressionValidator>
        <br />
        <asp:Label ID="EmailLabel" runat="server" Text="E-mail" /> :
        <asp:TextBox ID="Email" runat="server" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3"
                                    runat="server"
                                    ControlToValidate="Email"
                                    EnableClientScript="true">E-mail is required.</asp:RequiredFieldValidator>
        <asp:CustomValidator ID="CustomValidator3"
                             runat="server"
                             ControlToValidate="Email"
                             EnableClientScript="true"
                             ClientValidationFunction="ValidateEmail"
                             OnServerValidate="OnServerValidate_Email"
                             ErrorMessage="Must be a valid e-mail address.">Must be a valid e-mail address.</asp:CustomValidator>
        <br />
        <asp:Label ID="Address1Label" runat="server" Text="Address 1" /> :
        <asp:TextBox ID="Address1" runat="server" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator4"
                                    runat="server"
                                    ControlToValidate="Address1"
                                    EnableClientScript="true">Address 1 is required.</asp:RequiredFieldValidator>
        <br />
        <asp:Label ID="Address2Label" runat="server" Text="Address 2" /> :
        <asp:TextBox ID="Address2" runat="server" />
        <br />
        <asp:Label ID="CityLabel" runat="server" Text="City" /> :
        <asp:TextBox ID="City" runat="server" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator5"
                                    runat="server"
                                    ControlToValidate="City"
                                    EnableClientScript="true">City is required.</asp:RequiredFieldValidator>
        <br />
        <asp:Label ID="StateLabel" runat="server" Text="State" /> :
        <asp:DropDownList ID="State" runat="server" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator6"
                                    runat="server"
                                    ControlToValidate="State"
                                    EnableClientScript="true">State is required.</asp:RequiredFieldValidator>
        <br />
        <asp:Label ID="UserNameLabel" runat="server" Text="User Name" /> :
        <asp:TextBox ID="UserName" runat="server" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator7"
                                    runat="server"
                                    ControlToValidate="UserName"
                                    EnableClientScript="true">User Name is required.</asp:RequiredFieldValidator>
        <br />
        <asp:Label ID="PasswordLabel" runat="server" Text="User Name" /> :
        <asp:TextBox TextMode="Password" ID="Password" runat="server" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator8"
                                    runat="server"
                                    ControlToValidate="Password"
                                    EnableClientScript="true">Password is required.</asp:RequiredFieldValidator>
        <br />
        <asp:Label ID="ConfirmPasswordLabel" runat="server" Text="Confirm Password" /> :
        <asp:TextBox TextMode="Password" ID="ConfirmPassword" runat="server" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator9"
                                    runat="server"
                                    ControlToValidate="ConfirmPassword"
                                    EnableClientScript="true">Confirm Password is required.</asp:RequiredFieldValidator>
        <asp:CustomValidator ID="CustomValidator4"
                             runat="server"
                             ControlToValidate="ConfirmPassword"
                             EnableClientScript="true"
                             ClientValidationFunction="Validate_PasswordMatch"
                             OnServerValidate="OnServerValidate_PasswordMatch">Password and Confirm Password must match.</asp:CustomValidator>
        <br />
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_OnClick" /><br />
        <button onclick="document.forms[0].submit();">Force postback (ignore client-side validation)</button>
        <asp:Label ID="lblState" runat="server" Text="Initial State"></asp:Label>
        <br />
    </div>
    </form>
</body>
</html>
