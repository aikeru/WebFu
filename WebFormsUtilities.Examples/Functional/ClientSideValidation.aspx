<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientSideValidation.aspx.cs" Inherits="WebFormsUtilities.Examples.Functional.ClientSideValidation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <!-- These are just to make the examples look nice -->
    <script src="../Scripts/prettify.js" language="javascript" type="text/javascript"></script>
    <link href="../styles/prettify.css" rel="stylesheet" type="text/css" />
    <!-- These two scripts are required for WebFormsUtilities -->
    <script src="http://ajax.aspnetcdn.com/ajax/jquery/jquery-1.4.2.js" type="text/javascript"></script>
    <script src="../Scripts/WFUtilitiesJquery.js" type="text/javascript"></script>
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
<body onload="prettyPrint()">
    <form id="form1" runat="server">
    <div>
        <p>This example demonstrates the client-side validation available in WebFu.</p>
        <p>
            WebFu encourages the use of DataAnnotations. DataAnnotations are attributes added to .net 3.5 by Microsoft.<br />
            DataAnnotations can be added to the business layer and then recycled across projects, if desired.
        </p>
        
        <p>MSDN documentation for System.ComponentModel.DataAnnotations : <a href="http://msdn.microsoft.com/en-us/library/system.componentmodel.dataannotations.aspx">http://msdn.microsoft.com/en-us/library/system.componentmodel.dataannotations.aspx</a></p>
        <br />
        <br />
        <p>
            <h3>A typical WebForms page looks like this :</h3>
            Markup:<br />
            <pre class="prettyprint lang-html" style="font-size: 15px;">
            &lt;asp:Label runat="server" ID="lblFirstName" Text="First Name"&gt;&lt;/asp:Label&gt;
            &lt;asp:TextBox runat="server" ID="txtFirstName" Text=""&gt;&lt;/asp:TextBox&gt;
            &lt;asp:RequiredFieldValidator runat="server" ControlToValidate="txtFirstName"&gt;&lt;/asp:RequiredFieldValidator&gt;
            &lt;asp:RegularExpressionValidator runat="server" ControlToValidate="txtFirstName"&gt;&lt;/asp:RegularExpressionValidator&gt;
            &lt;asp:CustomValidator runat="server" ControlToValidate="txtFirstName"&gt;&lt;/asp:CustomValidator&gt;
            ...and so on for each property...
            </pre>
            <br />
            Code-behind:<br />
            Page_Load<br />
            <pre class="prettyprint lang-cs">
            if(!Page.IsPostBack) {
                txtFirstName.Text = Initial_Value;
                txtLastName.Text = Initial_Value;
                //...
            }
            </pre>
            btnSave_OnClick<br />
            <pre class="prettyprint lang-cs">
                Participant.FirstName = txtFirstName.Text;
                Participant.LastName = txtLastName.Text;
                //...
                Participant.Save();
            </pre>
            
            <br />
            <br />
            <h3>WebFormsUtilities allows you to do this :</h1>
            Markup:<br />
            <pre class="prettyprint lang-html" style="font-size: 15px;">
            &lt;%=Html.LabelFor(model => model.FirstName)%&gt;
            &lt;%=Html.TextBoxFor(model => model.FirstName)%&gt;
            &lt;%=Html.ValidationMessageFor(model => model.FirstName)%&gt;
            ...and so on for each property...
            &lt;!-- Enable Client-side validation --&gt;
            &lt;%=WFPageUtilities.EnableClientValidation(Html.MetaData) %&gt;
            &lt;!-- Or use the server control which performs the same thing as the above line: --&gt;
            &lt;WebFu:EnableClientValidationControl ID="EnableClientValidationControl1" runat="server" /&gt;
            </pre>
            <br />
            Code-behind:<br />
            Page_Load<br />
            <pre class="prettyprint lang-cs">
            if(Page.IsPostBack) {
               WFPageUtilities.UpdateModel&lt;Participant&gt;(Request, Participant); 
            }
            </pre>
            btnSave_OnClick<br />
            <pre class="prettyprint lang-cs">
                Participant.Save();
            </pre>
            
            <br />
            <br />
            
            <h3>Here is an example of client-side validation in action :</h3>
            <p><i>(only client-side validation has been enabled)</i></p>
            
            <div class="divExampleArea">
                <p>After successful postback, a label server control will be updated to show that the model has been saved.</p>
                <!-- This is where the summary of errors would be displayed if server side validation was enabled. -->
                <%=Html.ValidationSummary(Model) %>
            
                <!-- LabelFor is optional, and creates an HTML label for this field. -->
                <!-- It can also be used for globalization or to easily make changes to the name of a field across a project. -->
                <%=Html.LabelFor(model => Model.FirstName) %> : 
                <!-- Generate a TextBox for the FirstName property with validation enabled -->
                <%=Html.TextBoxFor(model => Model.FirstName) %>
                <!-- ValidationMessageFor is optional, since a ValidationSummary can be used -->
                <%=Html.ValidationMessageFor(model => Model.FirstName) %>
                <br />
                <%=Html.LabelFor(model => model.LastName) %>
                <%=Html.TextBoxFor(model => model.LastName) %>
                <%=Html.ValidationMessageFor(model => model.LastName) %>
                <br />
                <!-- WebFormsUtilities plays nice with server controls -->
                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_OnClick" /><br />
                <asp:Label ID="lblState" runat="server" Text="Initial State"></asp:Label>
            </div>
            
            
        </p>
    </div>
    <!-- This is required for client-side validation. Run this at the end of the form -->
    
    <%=WebFormsUtilities.WFPageUtilities.EnableClientValidation(Html.MetaData) %>
    </form>
</body>
</html>
