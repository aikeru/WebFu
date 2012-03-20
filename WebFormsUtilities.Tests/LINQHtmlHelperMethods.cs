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

        [TestMethod]
        public void TestLINQProperty() {
            TestParticipantClass tpc = GetTestParticipant();
            string lambda = Html.TextBoxFor(p => p.FirstName);
            Assert.AreEqual("<input name = \"FirstName\" id = \"FirstName\" type = \"text\" value = \"Michael\" />", lambda);

        }
        [TestMethod]
        public void TestLINQChildProperty() {
            TestParticipantClass tpc = GetTestParticipant();
            string lambdaChild = Html.TextBoxFor(p => p.Address.Address1);
            Assert.AreEqual("<input name = \"Address1\" id = \"Address1\" type = \"text\" value = \"101 somestreet\" />", lambdaChild);
        }
        [TestMethod]
        public void TestLINQChildObject() {
            TestParticipantClass tpc = GetTestParticipant();
            string lambdaObject = Html.TextBoxFor(p => p.Address);
            //Should evaluate to the .ToString() of the object WebFormsUtilities.Tests.TestParticipantAddressClass
            Assert.AreEqual("<input name = \"Address\" id = \"Address\" type = \"text\" value = \"" + tpc.Address.ToString() + "\" />", lambdaObject);
        }

        [TestMethod]
        public void TestLINQChildObjectNull() {
            TestParticipantClass tpc = GetTestParticipant();
            tpc.Address = null;
            string lambdaObject = Html.TextBoxFor(p => p.Address);
            //Should evaluate to "" since the object is null 
            Assert.AreEqual("<input name = \"Address\" id = \"Address\" type = \"text\" value = \"\" />", lambdaObject);
        }

        [TestMethod]
        public void TestLINQPropertyNull() {
            TestParticipantClass tpc = GetTestParticipant();
            tpc.FirstName = null;
            string lambdaObject = Html.TextBoxFor(p => p.FirstName);
            //Should evaluate to "" since the property is null 
            Assert.AreEqual("<input name = \"FirstName\" id = \"FirstName\" type = \"text\" value = \"\" />", lambdaObject);
        }


        [TestMethod]
        public void TestLINQCheckBoxFor() {
            TestParticipantClass tpc = GetTestParticipant();
            tpc.AcceptedRules = false;
            string lambdaStr = Html.CheckboxFor(p => p.AcceptedRules);
            Assert.AreEqual("<input name = \"AcceptedRules\" id = \"AcceptedRules\" type = \"checkbox\" />", lambdaStr);

            tpc.AcceptedRules = true;
            lambdaStr = Html.CheckboxFor(p => p.AcceptedRules);
            Assert.AreEqual("<input name = \"AcceptedRules\" id = \"AcceptedRules\" type = \"checkbox\" checked = \"checked\" />", lambdaStr);

            lambdaStr = Html.CheckboxFor(p => p.AcceptedRules, new { Value = "\"" });
            Assert.AreEqual("<input name = \"AcceptedRules\" id = \"AcceptedRules\" type = \"checkbox\" checked = \"checked\" value = \"&quot;\" />", lambdaStr);
        }

        private IEnumerable<SelectListItem> DropDownListForNoItems() {
            return new List<SelectListItem>();
        }
        private IEnumerable<SelectListItem> DropDownListForNoneSelected() {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Selected = false, Text = "item1", Value = "itemValue1" });
            items.Add(new SelectListItem() { Selected = false, Text = "item2", Value = "itemValue2" });
            items.Add(new SelectListItem() { Selected = false, Text = "item3", Value = "itemValue3" });
            return items;
        }
        private IEnumerable<SelectListItem> DropDownListForOneSelected() {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Selected = false, Text = "item1", Value = "itemValue1" });
            items.Add(new SelectListItem() { Selected = true, Text = "item2", Value = "itemValue2" });
            items.Add(new SelectListItem() { Selected = false, Text = "item3", Value = "itemValue3" });
            return items;
        }
        private IEnumerable<SelectListItem> DropDownListForManySelected() {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Selected = true, Text = "item1", Value = "itemValue1" });
            items.Add(new SelectListItem() { Selected = true, Text = "item2", Value = "itemValue2" });
            items.Add(new SelectListItem() { Selected = true, Text = "item3", Value = "itemValue3" });
            return items;
        }

        private IEnumerable<SelectListItem> DropDownListForManySelectedNeedEscape() {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Selected = true, Text = "item1>", Value = "itemValue1>" });
            items.Add(new SelectListItem() { Selected = true, Text = "item2>", Value = "itemValue2>" });
            items.Add(new SelectListItem() { Selected = true, Text = "item3>", Value = "itemValue3>" });
            return items;
        } 

        [TestMethod]
        public void TestLINQDropDownListFor() {
            TestParticipantClass tpc = GetTestParticipant();

            string lambdaStr = Html.DropDownListFor(p => p.LastName, DropDownListForNoneSelected());
            Assert.AreEqual("<select id = \"LastName\" name = \"LastName\"><option value = \"itemValue1\">item1</option>\r\n<option value = \"itemValue2\">item2</option>\r\n<option value = \"itemValue3\">item3</option>\r\n</select>\r\n", lambdaStr);
            lambdaStr = Html.DropDownListFor(p => p.LastName, DropDownListForNoItems());
            Assert.AreEqual("<select id = \"LastName\" name = \"LastName\"></select>\r\n", lambdaStr);
            lambdaStr = Html.DropDownListFor(p => p.LastName, DropDownListForNoItems(), new { Class = "ddlClass" });
            Assert.AreEqual("<select id = \"LastName\" name = \"LastName\" class = \"ddlClass\"></select>\r\n", lambdaStr);
            lambdaStr = Html.DropDownListFor(p => p.LastName, DropDownListForNoItems(), "No Item Selected", new { Class = "ddlClass" });
            Assert.AreEqual("<select id = \"LastName\" name = \"LastName\" class = \"ddlClass\"><option value = \"\">No Item Selected</option>\r\n</select>\r\n", lambdaStr);
            lambdaStr = Html.DropDownListFor(p => p.LastName, DropDownListForNoneSelected(), "No Item Selected", new { Class = "ddlClass" });
            Assert.AreEqual("<select id = \"LastName\" name = \"LastName\" class = \"ddlClass\"><option value = \"\">No Item Selected</option>\r\n<option value = \"itemValue1\">item1</option>\r\n<option value = \"itemValue2\">item2</option>\r\n<option value = \"itemValue3\">item3</option>\r\n</select>\r\n", lambdaStr);

            lambdaStr = Html.DropDownListFor(p => p.LastName, DropDownListForOneSelected(), "No Item Selected", new { Class = "ddlClass" });
            Assert.AreEqual("<select id = \"LastName\" name = \"LastName\" class = \"ddlClass\"><option value = \"\">No Item Selected</option>\r\n<option value = \"itemValue1\">item1</option>\r\n<option value = \"itemValue2\" selected = \"selected\">item2</option>\r\n<option value = \"itemValue3\">item3</option>\r\n</select>\r\n", lambdaStr);
            lambdaStr = Html.DropDownListFor(p => p.LastName, DropDownListForManySelected(), "No Item Selected", new { Class = "ddlClass" });
            Assert.AreEqual("<select id = \"LastName\" name = \"LastName\" class = \"ddlClass\"><option value = \"\">No Item Selected</option>\r\n<option value = \"itemValue1\" selected = \"selected\">item1</option>\r\n<option value = \"itemValue2\" selected = \"selected\">item2</option>\r\n<option value = \"itemValue3\" selected = \"selected\">item3</option>\r\n</select>\r\n", lambdaStr);

            lambdaStr = Html.DropDownListFor(p => p.LastName, DropDownListForManySelectedNeedEscape(), "No Item Selected", new { Class = "ddlClass" });
            Assert.AreEqual("<select id = \"LastName\" name = \"LastName\" class = \"ddlClass\"><option value = \"\">No Item Selected</option>\r\n<option value = \"itemValue1&gt;\" selected = \"selected\">item1&gt;</option>\r\n<option value = \"itemValue2&gt;\" selected = \"selected\">item2&gt;</option>\r\n<option value = \"itemValue3&gt;\" selected = \"selected\">item3&gt;</option>\r\n</select>\r\n", lambdaStr);
        }
        [TestMethod]
        public void TestLINQHiddenFor() {
            TestParticipantClass tpc = GetTestParticipant();

            string lambdaStr = Html.HiddenFor(p => p.FirstName);
            Assert.AreEqual("<input type = \"hidden\" value = \"Michael\" name = \"FirstName\" id = \"FirstName\" />", lambdaStr);

            tpc.FirstName = "&>";
            lambdaStr = Html.HiddenFor(p => p.FirstName);
            Assert.AreEqual("<input type = \"hidden\" value = \"&amp;&gt;\" name = \"FirstName\" id = \"FirstName\" />", lambdaStr);
        }
        [TestMethod]
        public void TestLINQLabelFor() {
            TestParticipantClass tpc = GetTestParticipant();

            string lambdaStr = Html.LabelFor(p => p.FirstName);
            Assert.AreEqual("<label for = \"FirstName\">FirstName</label>\r\n", lambdaStr);
        }
        [TestMethod]
        public void TestLINQRadioButtonFor() {
            TestParticipantClass tpc = GetTestParticipant();
            tpc.AcceptedRules = false;
            string lambdaStr = Html.RadioButtonFor(p => p.AcceptedRules);
            Assert.AreEqual("<input name = \"AcceptedRules\" id = \"AcceptedRules\" type = \"radio\" value = \"False\" />", lambdaStr);

            tpc.AcceptedRules = true;
            lambdaStr = Html.RadioButtonFor(p => p.AcceptedRules, new { Class = "rbClass" });
            Assert.AreEqual("<input name = \"AcceptedRules\" id = \"AcceptedRules\" type = \"radio\" value = \"True\" class = \"rbClass\" />", lambdaStr);

            lambdaStr = Html.RadioButtonFor(p => p.AcceptedRules, false, new { Class = "rbClass" });
            Assert.AreEqual("<input name = \"AcceptedRules\" id = \"AcceptedRules\" type = \"radio\" value = \"True\" class = \"rbClass\" />", lambdaStr);

            lambdaStr = Html.RadioButtonFor(p => p.AcceptedRules, true, new { Class = "rbClass" });
            Assert.AreEqual("<input name = \"AcceptedRules\" id = \"AcceptedRules\" type = \"radio\" value = \"True\" checked = \"checked\" class = \"rbClass\" />", lambdaStr);

            lambdaStr = Html.RadioButtonFor(p => p.AcceptedRules, true, new { Class = "rbClass", Value = "&gt;" });
            Assert.AreEqual("<input name = \"AcceptedRules\" id = \"AcceptedRules\" type = \"radio\" value = \"&amp;gt;\" checked = \"checked\" class = \"rbClass\" />", lambdaStr);
        }
        [TestMethod]
        public void TestLINQSpanFor() {
            TestParticipantClass tpc = GetTestParticipant();
            string lambdaStr = Html.SpanFor(p => p.FirstName);
            Assert.AreEqual("<span id = \"FirstName\">Michael</span>\r\n", lambdaStr);
            lambdaStr = Html.SpanFor(p => p.FirstName, new { Class = "spanClass" });
            Assert.AreEqual("<span id = \"FirstName\" class = \"spanClass\">Michael</span>\r\n", lambdaStr);

            tpc.FirstName = "&Michael";
            lambdaStr = Html.SpanFor(p => p.FirstName, new { Class = "spanClass" });
            Assert.AreEqual("<span id = \"FirstName\" class = \"spanClass\">&amp;Michael</span>\r\n", lambdaStr);
        }
        [TestMethod]
        public void TestLINQTextAreaFor() {
            TestParticipantClass tpc = GetTestParticipant();
            string lambdaStr = Html.TextAreaFor(p => p.FirstName);
            Assert.AreEqual("<textarea cols = \"20\" rows = \"2\" name = \"FirstName\" id = \"FirstName\">Michael</textarea>\r\n", lambdaStr);

            lambdaStr = Html.TextAreaFor(p => p.FirstName, new { Class = "txaClass" });
            Assert.AreEqual("<textarea cols = \"20\" rows = \"2\" name = \"FirstName\" id = \"FirstName\" class = \"txaClass\">Michael</textarea>\r\n", lambdaStr);

            lambdaStr = Html.TextAreaFor(p => p.FirstName, new { Class = "txaClass", rows = "5", cols = "25" });
            Assert.AreEqual("<textarea cols = \"25\" rows = \"5\" name = \"FirstName\" id = \"FirstName\" class = \"txaClass\">Michael</textarea>\r\n", lambdaStr);

            tpc.FirstName = "<Michael>";
            lambdaStr = Html.TextAreaFor(p => p.FirstName, new { Class = "txaClass", rows = "5", cols = "25" });
            Assert.AreEqual("<textarea cols = \"25\" rows = \"5\" name = \"FirstName\" id = \"FirstName\" class = \"txaClass\">&lt;Michael&gt;</textarea>\r\n", lambdaStr);
        }
        [TestMethod]
        public void TestLINQTextBoxFor() {
            TestParticipantClass tpc = GetTestParticipant();
            string lambdaStr = Html.TextBoxFor(p => p.FirstName);
            Assert.AreEqual("<input name = \"FirstName\" id = \"FirstName\" type = \"text\" value = \"Michael\" />", lambdaStr);
            lambdaStr = Html.TextBoxFor(p => p.FirstName, new { Class = "clsTxt" });
            Assert.AreEqual("<input name = \"FirstName\" id = \"FirstName\" type = \"text\" value = \"Michael\" class = \"clsTxt\" />", lambdaStr);

            //With an error
            tpc.FirstName = "";
            Html.MetaData = new WFModelMetaData(); //Reset metadata
            bool didValidate = WFUtilities.TryValidateModel(tpc, "", new WFObjectValueProvider(tpc, ""), Html.MetaData, new WFTypeRuleProvider(typeof(ProxyMessageFromValidator)));
            string lambdaError = Html.TextBoxFor(p => p.FirstName);
            Assert.AreEqual("<input name = \"FirstName\" id = \"FirstName\" type = \"text\" value = \"\" class = \"input-validation-error\" />", lambdaError);

            Html.MetaData = new WFModelMetaData(); //Reset metadata

            tpc.FirstName = "&Michael";
            lambdaStr = Html.TextBoxFor(p => p.FirstName, new { Class = "clsTxt" });
            Assert.AreEqual("<input name = \"FirstName\" id = \"FirstName\" type = \"text\" value = \"&amp;Michael\" class = \"clsTxt\" />", lambdaStr);
        }
        [TestMethod]
        public void TestLINQValidationMessageFor() {
            TestParticipantClass tpc = GetTestParticipant();

            string vmfOK = Html.ValidationMessageFor(m => m.FirstName);
            tpc.FirstName = "";
            Html.MetaData = new WFModelMetaData(); //Reset metadata
            bool didValidate = WFUtilities.TryValidateModel(tpc, "", new WFObjectValueProvider(tpc, ""), Html.MetaData, new WFTypeRuleProvider(typeof(ProxyMessageFromValidator)));
            string vmfError = Html.ValidationMessageFor(m => m.FirstName);

            Assert.AreEqual("<span id = \"FirstName_validationMessage\" name = \"FirstName_validationMessage\" class = \"field-validation-valid\"></span>\r\n", vmfOK);
            Assert.AreEqual("<span id = \"FirstName_validationMessage\" name = \"FirstName_validationMessage\" class = \"field-validation-error\">The FirstName field is required.</span>\r\n", vmfError);

            Html.MetaData = new WFModelMetaData(); //Reset metadata
        }



        [TestMethod]
        public void HtmlCheckboxFor() {

            TestParticipantClass tpc = GetTestParticipant();
            //AcceptedRules should be null (since it's a property and has not been set, even though it is non-nullable)
            Html.MetaData = new WFModelMetaData(); //Reset metadata
            string boolnull = Html.CheckboxFor(c => c.AcceptedRules);
            Assert.AreEqual("<input name = \"AcceptedRules\" id = \"AcceptedRules\" type = \"checkbox\" />", boolnull);
            tpc.AcceptedRules = false;
            boolnull = Html.CheckboxFor(c => c.AcceptedRules);
            Assert.AreEqual("<input name = \"AcceptedRules\" id = \"AcceptedRules\" type = \"checkbox\" />", boolnull);

            tpc.AcceptedRules = true;
            string booltrue = Html.CheckboxFor(c => c.AcceptedRules);
            Assert.AreEqual("<input name = \"AcceptedRules\" id = \"AcceptedRules\" type = \"checkbox\" checked = \"checked\" />", booltrue);
            Html.MetaData = new WFModelMetaData(); //Reset metadata

            boolnull = Html.CheckboxFor(c => c.AcceptedRules, new { Value = ">" });
            Assert.AreEqual("<input name = \"AcceptedRules\" id = \"AcceptedRules\" type = \"checkbox\" checked = \"checked\" value = \"&gt;\" />", boolnull);
        }

        private TestParticipantClass _Participant = null;
        private TestParticipantClass GetTestParticipant() {
            if (_Participant == null) {
                _Participant = new TestParticipantClass() {
                    FirstName = "Michael",
                    LastName = "Snead"
                };
                _Participant.Address = new TestParticipantAddressClass();
                _Participant.Address.Address1 = "101 somestreet";
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
