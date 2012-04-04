<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyAccountServerControlsXML.aspx.cs" Inherits="WebFormsUtilities.Examples.MyAccount.MyAccountServerControlsXML" %>

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
        <!-- The first 4 properties come from the Customer object. The DataAnnotation rules
             used are determined from XmlRuleSet "RegisterRules" which is setup in Global.asax.cs -->
        <WebFu:LabelFor ID="FirstNameLabel" runat="server" TargetControl="FirstName" PropertyName="FirstName" XmlRuleSetName="RegisterRules" /> :
        <asp:TextBox ID="FirstName" runat="server" />
        
        <WebFu:DataAnnotationValidatorControl ID="DataAnnotationValidatorControl1"
                                              runat="server"
                                              ControlToValidate="FirstName"
                                              PropertyName="FirstName"
                                              XmlRuleSetName="RegisterRules"
                                              CssClass="testcssclass"
                                              class="someclass"
                                              attrib1="somevalue"
                                              attrib2="somevalue" />
        <br />
        <WebFu:LabelFor ID="LastNameLabel" runat="server" TargetControl="LastName" PropertyName="LastName" XmlRuleSetName="RegisterRules" /> :
        <asp:TextBox ID="LastName" runat="server" />
        <WebFu:DataAnnotationValidatorControl ID="DataAnnotationValidatorControl2"
                                              runat="server"
                                              ControlToValidate="LastName"
                                              PropertyName="LastName"
                                              XmlRuleSetName="RegisterRules" />
        <br />
        <WebFu:LabelFor ID="EmailLabel" runat="server" TargetControl="Email" PropertyName="Email" XmlRuleSetName="RegisterRules" /> :
        <asp:TextBox ID="Email" runat="server" />
        <WebFu:DataAnnotationValidatorControl ID="DataAnnotationValidatorControl3"
                                              runat="server"
                                              ControlToValidate="Email"
                                              PropertyName="Email"
                                              XmlRuleSetName="RegisterRules" />   
        <br />
        <WebFu:LabelFor ID="SocialSecurityNumberLabel" runat="server" TargetControl="SocialSecurityNumber" PropertyName="SocialSecurityNumber" XmlRuleSetName="RegisterRules" /> :
        <asp:TextBox ID="SocialSecurityNumber" runat="server" />
        <WebFu:DataAnnotationValidatorControl ID="DataAnnotationValidatorControl4"
                                              runat="server"
                                              ControlToValidate="SocialSecurityNumber"
                                              PropertyName="SocialSecurityNumber"
                                              XmlRuleSetName="RegisterRules" />  
        <br />
        <!-- Properties from the Customer.Address object have a PropertyName of Address.* -->
        <WebFu:LabelFor ID="Address1Label" 
                        runat="server" 
                        TargetControl="Address1"
                        PropertyName="Address.Address1"
                        XmlRuleSetName="RegisterRules" /> :
        <asp:TextBox ID="Address1" runat="server" />
        <WebFu:DataAnnotationValidatorControl ID="DataAnnotationValidatorControl5"
                                              runat="server"
                                              ControlToValidate="Address1"
                                              PropertyName="Address.Address1"
                                              XmlRuleSetName="RegisterRules" />         
        <br />
        <WebFu:LabelFor ID="Address2Label" 
                        runat="server" 
                        TargetControl="Address2"
                        PropertyName="Address.Address2"
                        XmlRuleSetName="RegisterRules" /> :
        <asp:TextBox ID="Address2" runat="server" />
        <WebFu:DataAnnotationValidatorControl ID="DataAnnotationValidatorControl6"
                                              runat="server"
                                              ControlToValidate="Address2"
                                              PropertyName="Address.Address2"
                                              XmlRuleSetName="RegisterRules" />         
        <br />
        <WebFu:LabelFor ID="CityLabel" 
                        runat="server" 
                        TargetControl="City"
                        PropertyName="Address.City"
                        XmlRuleSetName="RegisterRules" /> :
        <asp:TextBox ID="City" runat="server" />
        <WebFu:DataAnnotationValidatorControl ID="DataAnnotationValidatorControl7"
                                              runat="server"
                                              ControlToValidate="City"
                                              PropertyName="Address.City"
                                              XmlRuleSetName="RegisterRules" />     
        <br />
        <WebFu:LabelFor ID="StateLabel" 
                        runat="server" 
                        TargetControl="State"
                        PropertyName="Address.State"
                        XmlRuleSetName="RegisterRules" /> :
        <asp:DropDownList ID="State" runat="server"></asp:DropDownList>        
        <WebFu:DataAnnotationValidatorControl ID="DataAnnotationValidatorControl8"
                                              runat="server"
                                              ControlToValidate="State"
                                              PropertyName="Address.State"
                                              XmlRuleSetName="RegisterRules" />  
        <br />
        <!-- Properties from the Customer.Login object have a PropertyName of Login.* -->
        <WebFu:LabelFor ID="UserNameLabel" 
                        runat="server" 
                        TargetControl="UserName"
                        PropertyName="Login.UserName"
                        XmlRuleSetName="RegisterRules" /> :
        <asp:TextBox ID="UserName" runat="server" />
        <WebFu:DataAnnotationValidatorControl ID="DataAnnotationValidatorControl9"
                                              runat="server"
                                              ControlToValidate="UserName"
                                              PropertyName="Login.UserName"
                                              XmlRuleSetName="RegisterRules" /> 
        <br />
        <WebFu:LabelFor ID="PasswordLabel" 
                        runat="server" 
                        TargetControl="Password"
                        PropertyName="Login.Password"
                        XmlRuleSetName="RegisterRules" /> :
        <asp:TextBox TextMode="Password" ID="Password" runat="server" />
        <WebFu:DataAnnotationValidatorControl ID="DataAnnotationValidatorControl10"
                                              runat="server"
                                              ControlToValidate="Password"
                                              PropertyName="Login.Password"
                                              XmlRuleSetName="RegisterRules" />
        <br />
        <WebFu:LabelFor ID="CoonfirmPasswordLabel" 
                        runat="server" 
                        TargetControl="ConfirmPassword"
                        PropertyName="Login.ConfirmPassword"
                        XmlRuleSetName="RegisterRules" /> :
        <asp:TextBox ID="ConfirmPassword" runat="server" />
        <WebFu:DataAnnotationValidatorControl ID="DataAnnotationValidatorControl11"
                                              runat="server"
                                              ControlToValidate="ConfirmPassword"
                                              PropertyName="Login.ConfirmPassword"
                                              XmlRuleSetName="RegisterRules" />
        <br />
        <!-- WebFormsUtilities plays nice with server controls -->
        <asp:Button ID="btnSubmit" runat="server" Text="Save" OnClick="btnSave_OnClick" /><br />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_OnClick" /><br />
        <br />
    </div>
    <!-- This is required for client-side validation. Run this at the end of the form -->
    <WebFu:EnableClientValidationControl ID="EnableClientValidationControl1" runat="server" />
    </form>
</body>
</html>
