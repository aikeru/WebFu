using WebFormsUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebFormsUtilities.DataAnnotations;
using System.IO;
using WebFormsUtilities.Json;
using System.Reflection;
using System.Linq;
using System.Collections;
using WebFormsUtilities.Tests.Properties;
using WebFormsUtilities.ValueProviders;
using WebFormsUtilities.RuleProviders;
using WebFormsUtilities.Tests.TestObjects;
using WebFormsUtilities.WebControls;
using System.Web.UI.WebControls;
using System.Web.UI;


namespace WebFormsUtilities.Tests
{
    
    
    /// <summary>
    ///This is a test class for WFUtilitiesTest and is intended
    ///to contain all WFUtilitiesTest Unit Tests
    ///</summary>
    [TestClass()]
    public class WFUtilitiesTest {
        #region WFUtilities.RegisterXMLValidationConfiguration()
        [TestMethod]
        [DeploymentItem("Validator.config")]
        public void RegisterXMLValidationConfiguration_TestLoadXML() {
            //
            // TODO: Add test logic	here
            //

            WFUtilities.RegisterXMLValidationConfiguration(Environment.CurrentDirectory + "\\Validator.config");

            Assert.AreEqual(1, WFUtilities.XmlRuleSets.Count());
            Assert.AreEqual(4, WFUtilities.XmlRuleSets[0].Properties.Count);
            Assert.AreEqual(2, WFUtilities.XmlRuleSets[0].Properties[0].Validators.Count);

        }

        [TestMethod]
        [DeploymentItem("Validator2.config")]
        public void RegisterXMLValidationConfiguration_TestXMLErrorMessage() {
            WFUtilities.RegisterXMLValidationConfiguration(Environment.CurrentDirectory + "\\Validator2.config");

            string resourceErrorMessage = Resources.FirstName_ErrorMessage_Test1;

            TestParticipantClass tpc = new TestParticipantClass();

            WFModelMetaData metadata = new WFModelMetaData();


            //============================= WEB FORMS SERVER CONTROL VALIDATION =================================
            Page p = new Page();
            p.Controls.Add(new TextBox() { ID = "FirstName" });
            p.Controls.Add(new DataAnnotationValidatorControl() {
                PropertyName = "FirstName",
                ControlToValidate = "FirstName",
                XmlRuleSetName = "MessageFromValidator",
                ErrorMessage = "This message from control itself."
            });

            p.Validators.Add(p.Controls[1] as IValidator);


            (p.Controls[1] as BaseValidator).Validate();
            Assert.AreEqual("This message from control itself.", (p.Controls[1] as DataAnnotationValidatorControl).ErrorMessage);

            p = new Page();
            p.Controls.Add(new TextBox() { ID = "FirstName" });
            p.Controls.Add(new DataAnnotationValidatorControl() {
                PropertyName = "FirstName",
                ControlToValidate = "FirstName",
                XmlRuleSetName = "MessageFromXML",
            });

            p.Validators.Add(p.Controls[1] as IValidator);


            (p.Controls[1] as BaseValidator).Validate();
            Assert.AreEqual("The First Name field cannot be empty.", (p.Controls[1] as DataAnnotationValidatorControl).ErrorMessage);

            p = new Page();
            p.Controls.Add(new TextBox() { ID = "FirstName" });
            p.Controls.Add(new DataAnnotationValidatorControl() {
                PropertyName = "FirstName",
                ControlToValidate = "FirstName",
                XmlRuleSetName = "MessageFromResource",
            });

            p.Validators.Add(p.Controls[1] as IValidator);


            (p.Controls[1] as BaseValidator).Validate();
            Assert.AreEqual("This value from resource.", (p.Controls[1] as DataAnnotationValidatorControl).ErrorMessage);

            //====================== TryValidateModel ==============================

            WFUtilities.TryValidateModel(tpc, "", new WFObjectValueProvider(tpc, ""), metadata, new WFXmlRuleSetRuleProvider("MessageFromValidator"));
            Assert.AreEqual("The FirstName field is required.", metadata.Errors[0]);
            metadata = new WFModelMetaData();
            WFUtilities.TryValidateModel(tpc, "", new WFObjectValueProvider(tpc, ""), metadata, new WFXmlRuleSetRuleProvider("MessageFromXML"));
            Assert.AreEqual("The First Name field cannot be empty.", metadata.Errors[0]);
            metadata = new WFModelMetaData();
            WFUtilities.TryValidateModel(tpc, "", new WFObjectValueProvider(tpc, ""), metadata, new WFXmlRuleSetRuleProvider("MessageFromResource"));
            Assert.AreEqual("This value from resource.", metadata.Errors[0]);
            metadata = new WFModelMetaData();


        }

        [TestMethod]
        [DeploymentItem("Validator3.config")]
        public void RegisterXMLValidationConfiguration_TestClassLevelValidators() {
            WFUtilities.RegisterXMLValidationConfiguration(Environment.CurrentDirectory + "\\Validator3.config");
            TestParticipantClass tpc = new TestParticipantClass();
            tpc.Password = "onevalue";
            tpc.ConfirmPassword = "othervalue";
            WFModelMetaData metadata = new WFModelMetaData();
            WFUtilities.TryValidateModel(tpc, "", new WFObjectValueProvider(tpc, ""), metadata, new WFXmlRuleSetRuleProvider("ClassLevelAttributes"));

            Assert.AreEqual("Password fields must match.", metadata.Errors[0]);

        }

        #endregion

        [TestMethod()]
        public void UrlDecodeDictionary_Tests() {
            Dictionary<string, string> request = WFUtilities.UrlDecodeDictionary("");
            Assert.AreEqual(0, request.Count);
            request = WFUtilities.UrlDecodeDictionary("name=value");
            Assert.AreEqual(1, request.Count);
            Assert.AreEqual("value", request["name"]);
            request = WFUtilities.UrlDecodeDictionary("name=value&name2=value2");
            Assert.AreEqual(2, request.Count);
            Assert.AreEqual("value", request["name"]);
            Assert.AreEqual("value2", request["name2"]);
            request = WFUtilities.UrlDecodeDictionary("name=value&name=value2");
            Assert.AreEqual(1, request.Count);
            Assert.AreEqual("value,value2", request["name"]);
            request = WFUtilities.UrlDecodeDictionary("&&name=value");
            Assert.AreEqual(1, request.Count);
            Assert.AreEqual("value", request["name"]);
            request = WFUtilities.UrlDecodeDictionary("name=value&");
            Assert.AreEqual(1, request.Count);
            Assert.AreEqual("value", request["name"]);
            request = WFUtilities.UrlDecodeDictionary("name&value");
            Assert.AreEqual(2, request.Count);
            Assert.AreEqual("", request["name"]);
            Assert.AreEqual("", request["value"]);
            request = WFUtilities.UrlDecodeDictionary("name&name2=value2");
            Assert.AreEqual(2, request.Count);
            Assert.AreEqual("", request["name"]);
            Assert.AreEqual("value2", request["name2"]);
            request = WFUtilities.UrlDecodeDictionary("name=value&name2");
            Assert.AreEqual(2, request.Count);
            Assert.AreEqual("value", request["name"]);
            Assert.AreEqual("", request["name2"]);
        }

        [TestMethod()]
        public void TryValidateModel_ErrorMessageTests() {
            string resourceErrorMessage = Resources.FirstName_ErrorMessage_Test1;

            TestParticipantClass tpc = new TestParticipantClass();

            WFModelMetaData metadata = new WFModelMetaData();

            WFUtilities.TryValidateModel(tpc, "", new WFObjectValueProvider(tpc, ""), metadata, new WFTypeRuleProvider(typeof(ProxyMessageFromValidator)));
            Assert.AreEqual("The FirstName field is required.", metadata.Errors[0]);
            metadata = new WFModelMetaData();
            WFUtilities.TryValidateModel(tpc, "", new WFObjectValueProvider(tpc, ""), metadata, new WFTypeRuleProvider(typeof(ProxyMessageFromConstant)));
            Assert.AreEqual("This is a constant error.", metadata.Errors[0]);
            metadata = new WFModelMetaData();
            WFUtilities.TryValidateModel(tpc, "", new WFObjectValueProvider(tpc, ""), metadata, new WFTypeRuleProvider(typeof(ProxyMessageFromResource)));
            Assert.AreEqual("This value from resource.", metadata.Errors[0]);
            metadata = new WFModelMetaData();
        }

        /// <summary>
        ///A test for TryValidateModel
        ///</summary>
        [TestMethod()]
        public void TryValidateModel_TestAgainstModel()
        {
            TestWidget model = new TestWidget();
            Type modelType = model.GetType();
            List<string> errors = null; 
            List<string> errorsExpected = new List<string>();
            WFModelMetaData metadata = new WFModelMetaData(); 
            WFModelMetaData metadataExpected = new WFModelMetaData();
            bool expected = false;
            bool actual;

            model.Email = "bademail";
            model.MaxLengthName = "toolongofaname";
            model.RequiredName = ""; //Required
            model.Sprockets = 4; //Invalid sprocket count
            model.Price = 999; //Invalid price
            model.NoErrorMessage = ""; //Required - no error message defined

            actual = WFUtilities.TryValidateModel(model, "", new WFObjectValueProvider(model, ""), metadata, new WFTypeRuleProvider(modelType));
            errors = metadata.Errors;

            Assert.AreEqual(errors.Count, 6);
            //Generated error messages on standard annotations
            Assert.AreEqual(errors[0], "Widget name required");
            Assert.AreEqual(errors[1], "Max length 10 characters");
            Assert.AreEqual(errors[2], "Invalid number of sprockets.");
            Assert.AreEqual(errors[3], "Widget must have valid e-mail");
            //Generated error message on custom annotation
            Assert.AreEqual(errors[4], "Invalid price");
            //Auto-generated error message
            Assert.AreEqual(errors[5], "The NoErrorMessage field is required.");

            Assert.AreEqual(metadata.Properties.Count, 6);
            Assert.AreEqual(metadata.Properties[0].PropertyName, "RequiredName");
            Assert.AreEqual(metadata.Properties[1].PropertyName, "MaxLengthName");
            Assert.AreEqual(metadata.Properties[2].PropertyName, "Sprockets");
            Assert.AreEqual(metadata.Properties[3].PropertyName, "Email");
            Assert.AreEqual(metadata.Properties[4].PropertyName, "Price");
            Assert.AreEqual(metadata.Properties[5].PropertyName, "NoErrorMessage");

            Assert.AreEqual(actual, false);
        }
        [TestMethod()]
        public void TryValidateModel_TestAgainstModelSuccess()
        {
            TestWidget model = new TestWidget();
            Type modelType = model.GetType();
            WFModelMetaData metadata = new WFModelMetaData();
            WFModelMetaData metadataExpected = new WFModelMetaData();
            bool actual;

            model.Email = "goodemail@mail.com";
            model.MaxLengthName = "goodname";
            model.RequiredName = "reqname"; //Required
            model.Sprockets = 6; //Good sprocket count
            model.Price = 0.99d; //Good price
            model.NoErrorMessage = "reqmsg"; //Required - no error message defined

            actual = WFUtilities.TryValidateModel(model, "", new WFObjectValueProvider(model, ""), metadata, new WFTypeRuleProvider(modelType));

            //No Errors
            Assert.AreEqual(metadata.Errors.Count, 0);
            
            //Properties are not collected when an error does not exist
            //The reason is because there is no page to collect them for
            Assert.AreEqual(metadata.Properties.Count, 0);
           
            Assert.AreEqual(actual, true);
        }

        [TestMethod()]
        public void TryValidateModel_TestAgainstModelClassAttribute()
        {
            ClassAttributeTestWidget model = new ClassAttributeTestWidget();
            Type modelType = model.GetType();
            WFModelMetaData metadata = new WFModelMetaData();
            WFModelMetaData metadataExpected = new WFModelMetaData();
            bool actual;

            model.Email = "goodemail@mail.com";
            model.MaxLengthName = "goodname";
            model.RequiredName = "reqname"; //Required
            model.Sprockets = 6; //Good sprocket count
            model.Price = 0.99d; //Good price
            model.NoErrorMessage = "reqmsg"; //Required - no error message defined
            model.Password = "somepass";
            model.ConfirmPassword = "notthesame";
            actual = WFUtilities.TryValidateModel(model, "", new WFObjectValueProvider(model, ""), metadata, new WFTypeRuleProvider(modelType));

            //There should be one error from the model attribute
            Assert.AreEqual(metadata.Errors.Count, 1);

            //Properties are not collected when an error does not exist
            //The reason is because there is no page to collect them for
            Assert.AreEqual(metadata.Properties.Count, 0);

            Assert.AreEqual(actual, false);
        }

        [TestMethod()]
        public void TryValidateModel_TestAJAXLikeValidation()
        {
            TestWidget model = new TestWidget();
            Type modelType = model.GetType();
            List<string> errors = null;
            WFModelMetaData metadata = new WFModelMetaData();
            WFModelMetaData metadataExpected = new WFModelMetaData();
            bool actual;
            Dictionary<string, string> values = new Dictionary<string,string>();

            values.Add("model_RequiredName", "reqname");
            values.Add("model_MaxLengthName", "toolongofaname");
            values.Add("model_Sprockets", "4"); //bad sprocket value
            values.Add("model_Email", "bademail"); //bad email
            values.Add("model_Price", "999"); //bad price
            // NoErrorMessage property is NOT added and should NOT be present

            actual = WFUtilities.TryValidateModel(model, "model_", new WFDictionaryValueProvider(values), metadata, new WFTypeRuleProvider(modelType));
            errors = metadata.Errors;
            Assert.AreEqual(errors.Count, 4);

            Assert.AreEqual(errors[0], "Max length 10 characters");
            Assert.AreEqual(errors[1], "Invalid number of sprockets.");
            Assert.AreEqual(errors[2], "Widget must have valid e-mail");
            //Generated error message on custom annotation
            Assert.AreEqual(errors[3], "Invalid price");

            Assert.AreEqual(actual, false);
            
        }

        [TestMethod()]
        public void TryValidateModel_TestProxyClassValidation()
        {
            FriendlyClass fc = new FriendlyClass();
            fc.LastName = "onereallybiglongstringthatkeepsgoing"; // break validation
            List<string> errors = null;
            WFModelMetaData metadata = new WFModelMetaData();
            bool didValidate = WFUtilities.TryValidateModel(fc, "", new WFObjectValueProvider(fc, ""), metadata, new WFTypeRuleProvider(typeof(FriendlyClass)));
            errors = metadata.Errors;
            Assert.AreEqual(false, didValidate);
            Assert.AreEqual(2, errors.Count);
            Assert.AreEqual("The FirstName field is required.", errors[0]);
            Assert.AreEqual("The field LastName must be a string with a maximum length of 10.", errors[1]);
            Assert.AreEqual(2, metadata.Properties.Count);

        }

        [TestMethod()]
        public void TryValidateModel_TestPage()
        {
            //TODO: Add a unit test for this
            // Not sure how this can be tested as it uses HttpContext
            // Perhaps with a refactor the form values can be extracted
            // and the functionality can be tested separately.
            Assert.Inconclusive();
        }

    }
    

    

    

}
