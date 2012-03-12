using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using WebFormsUtilities.Tests.Properties;
using WebFormsUtilities.WebControls;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFormsUtilities.ValueProviders;
using WebFormsUtilities.RuleProviders;

namespace WebFormsUtilities.Tests {
    /// <summary>
    /// Summary description for XmlRuleSetTests
    /// </summary>
    [TestClass]
    public class XmlRuleSetTests {
        public XmlRuleSetTests() {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext {
            get {
                return testContextInstance;
            }
            set {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        [DeploymentItem("Validator.config")]
        public void TestLoadXML() {
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
        public void TestXMLErrorMessage() {
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
        public void TestClassLevelValidators() {
            WFUtilities.RegisterXMLValidationConfiguration(Environment.CurrentDirectory + "\\Validator3.config");
            TestParticipantClass tpc = new TestParticipantClass();
            tpc.Password = "onevalue";
            tpc.ConfirmPassword = "othervalue";
            WFModelMetaData metadata = new WFModelMetaData();
            WFUtilities.TryValidateModel(tpc, "", new WFObjectValueProvider(tpc, ""), metadata, new WFXmlRuleSetRuleProvider("ClassLevelAttributes"));

            Assert.AreEqual("Password fields must match.", metadata.Errors[0]);

        }
    }
}
