using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;
using WebFormsUtilities.RuleProviders;
using WebFormsUtilities.ValueProviders;
using WebFormsUtilities.TagProcessors;

namespace WebFormsUtilities.Tests {
    /// <summary>
    /// Summary description for TestValidationMarkup
    /// </summary>
    [TestClass]
    public class TestValidationMarkup : System.Web.UI.Page, IWebFormsView<TestValidationMarkupParticipant> { 
        public TestValidationMarkup() {
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

        //TODO: MVC/LAMBDA
        //[TestMethod]
        //public void TestValidationPreProcessorFor() {
        //    TestParticipant.Address = new TestValidationMarkupParticipantAddress();
        //    TestParticipant.FirstName = "waytoolongstringthatistoolong";
        //    TestParticipant.Address.Address1 = "waytoolongstringthatistoolong";

        //    string markupText = Html.TextBoxFor("FirstName", "FirstName", TestParticipant, TestParticipant.FirstName);
        //    WFUtilities.TryValidateModel(TestParticipant, "", new WFObjectValueProvider(TestParticipant, ""), Html.MetaData, new WFTypeRuleProvider(typeof(TestValidationMarkupParticipant)));
        //    string markupTextAfter1 = Html.TextBoxFor("FirstName", "FirstName", TestParticipant, TestParticipant.FirstName);
        //    string markupTextAfter2 = Html.TextAreaFor("FirstName", "FirstName", TestParticipant, TestParticipant.FirstName);

        //    string markupTextAfter3 = Html.TextBoxFor(model => model.FirstName);
        //    string markupTextAfter4 = Html.TextAreaFor(model => model.FirstName);

        //    Assert.AreEqual("<input type = \"text\" name = \"FirstName\" value = \"waytoolongstringthatistoolong\" id = \"FirstName\" class = \"input-validation-error\" />", markupTextAfter1);
        //    Assert.AreEqual("<textarea cols = \"20\" rows = \"2\" name = \"FirstName\" id = \"FirstName\" class = \"input-validation-error\">waytoolongstringthatistoolong</textarea>\r\n", markupTextAfter2);
        //    Assert.AreEqual("<input name = \"FirstName\" id = \"FirstName\" type = \"text\" value = \"waytoolongstringthatistoolong\" class = \"input-validation-error\" />", markupTextAfter3);
        //    Assert.AreEqual("<textarea cols = \"20\" rows = \"2\" name = \"FirstName\" id = \"FirstName\" class = \"input-validation-error\">waytoolongstringthatistoolong</textarea>\r\n", markupTextAfter4);

        //    //Test after clearing
        //    Html.ClearPreProcessors();

        //    string markupTextNoValidation = Html.TextBoxFor("FirstName", "FirstName", TestParticipant, TestParticipant.FirstName);

        //    Assert.AreEqual(markupText, markupTextNoValidation);

        //    //Add it back for any later tests
        //    Html.AddPreProcessor(new WFValidationPreProcessor());

        //}

        private TestValidationMarkupParticipant TestParticipant = new TestValidationMarkupParticipant();

        #region IWebFormsView<TestParticipantClass> Members

        public TestValidationMarkupParticipant Model {
            get {
                return TestParticipant;
            }
            set {
                throw new NotImplementedException("Not needed??");
            }
        }
        HtmlHelper<TestValidationMarkupParticipant> _Html = null;
        public HtmlHelper<TestValidationMarkupParticipant> Html {
            get {
                if (_Html == null) {
                    _Html = new HtmlHelper<TestValidationMarkupParticipant>(this);
                }
                return _Html;
            }
        }

        #endregion

    }

    public class TestValidationMarkupParticipant {
        [StringLength(10)]
        public string FirstName { get; set; }
        public TestValidationMarkupParticipantAddress Address { get; set; }
    }
    public class TestValidationMarkupParticipantAddress {
        [StringLength(10)]
        public string Address1 { get; set; }
    }
  
}
