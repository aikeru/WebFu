using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebFormsUtilities.RuleProviders;
using WebFormsUtilities.ValueProviders;

namespace WebFormsUtilities.Tests {
    /// <summary>
    /// Summary description for LINQHtmlHelperMethods
    /// </summary>
    [TestClass]
    public class LINQHtmlHelperMethods : System.Web.UI.Page, IWebFormsView<TestParticipantClass> {
        public LINQHtmlHelperMethods() {
            //
            // TODO: Add constructor logic here
            //
        }

        HtmlHelper<TestParticipantClass> _Html = null;

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

        //TODO: MVC/LAMBDA
        //[TestMethod]
        //public void HtmlInputFor() {
        //    TestParticipantClass tpc = GetTestParticipant();

        //    //Without an error
        //    string lambda = Html.TextBoxFor(p => p.FirstName);
        //    Assert.AreEqual("<input name = \"FirstName\" id = \"FirstName\" type = \"text\" value = \"Michael\" />", lambda);

        //    //With an error
        //    tpc.FirstName = "";
        //    Html.MetaData = new WFModelMetaData(); //Reset metadata
        //    bool didValidate = WFUtilities.TryValidateModel(tpc, "", new WFObjectValueProvider(tpc, ""), Html.MetaData, new WFTypeRuleProvider(typeof(ProxyMessageFromValidator)));
        //    string lambdaError = Html.TextBoxFor(p => p.FirstName);
        //    Assert.AreEqual("<input name = \"FirstName\" id = \"FirstName\" type = \"text\" value = \"\" class = \"input-validation-error\" />", lambdaError);

        //    Html.MetaData = new WFModelMetaData(); //Reset metadata
        //}

        //TODO: MVC/LAMBDA
        //[TestMethod]
        //public void HtmlValidationMessageFor() {
        //    TestParticipantClass tpc = GetTestParticipant();

        //    string vmfOK = Html.ValidationMessageFor(m => m.FirstName);
        //    tpc.FirstName = "";
        //    Html.MetaData = new WFModelMetaData(); //Reset metadata
        //    bool didValidate = WFUtilities.TryValidateModel(tpc, "", new WFObjectValueProvider(tpc, ""), Html.MetaData, new WFTypeRuleProvider(typeof(ProxyMessageFromValidator)));
        //    string vmfError = Html.ValidationMessageFor(m => m.FirstName);

        //    Assert.AreEqual("<span id = \"FirstName_validationMessage\" name = \"FirstName_validationMessage\" class = \"field-validation-valid\"></span>\r\n", vmfOK);
        //    Assert.AreEqual("<span id = \"FirstName_validationMessage\" name = \"FirstName_validationMessage\" class = \"field-validation-error\">The FirstName field is required.</span>\r\n", vmfError);

        //    Html.MetaData = new WFModelMetaData(); //Reset metadata

        //}

        //TODO: MVC/LAMBDA
        //[TestMethod]
        //public void HtmlCheckboxFor() {

        //    TestParticipantClass tpc = GetTestParticipant();
        //    //AcceptedRules should be null (since it's a property and has not been set, even though it is non-nullable)
        //    Html.MetaData = new WFModelMetaData(); //Reset metadata
        //    string boolnull = Html.CheckboxFor(c => c.AcceptedRules);
        //    Assert.AreEqual("<input name = \"AcceptedRules\" id = \"AcceptedRules\" type = \"checkbox\" />", boolnull);
        //    tpc.AcceptedRules = false;
        //    boolnull = Html.CheckboxFor(c => c.AcceptedRules);
        //    Assert.AreEqual("<input name = \"AcceptedRules\" id = \"AcceptedRules\" type = \"checkbox\" />", boolnull);

        //    tpc.AcceptedRules = true;
        //    string booltrue = Html.CheckboxFor(c => c.AcceptedRules);
        //    Assert.AreEqual("<input name = \"AcceptedRules\" id = \"AcceptedRules\" type = \"checkbox\" checked = \"checked\" />", booltrue);
        //    Html.MetaData = new WFModelMetaData(); //Reset metadata
        //}

        private TestParticipantClass _Participant = null;
        private TestParticipantClass GetTestParticipant() {
            if (_Participant == null) {
                _Participant = new TestParticipantClass() {
                FirstName = "Michael",
                LastName = "Snead"
                };
            }
            return _Participant;
        }

        #region IWebFormsView<TestParticipantClass> Members

        public TestParticipantClass Model {
            get {
                return GetTestParticipant();
            }
            set {
                throw new NotImplementedException("Not needed??");    
            }
        }

        public HtmlHelper<TestParticipantClass> Html {
            get {
                if (_Html == null) {
                    _Html = new HtmlHelper<TestParticipantClass>(this);
                }
                return _Html;
            }
        }

        #endregion
    }
}
