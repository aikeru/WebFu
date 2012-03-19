﻿using WebFormsUtilities;
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


namespace WebFormsUtilities.Tests
{
    
    
    /// <summary>
    ///This is a test class for WFUtilitiesTest and is intended
    ///to contain all WFUtilitiesTest Unit Tests
    ///</summary>
    [TestClass()]
    public class WFUtilitiesTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        [TestMethod()]
        public void TestURLDecodeDictionary() {
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
        public void TestWFErrorMessage() {
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
        public void TryValidateModelTestAgainstModel()
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
        public void TryValidateModelTestAgainstModelSuccess()
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
        public void TryValidateModelTestAgainstModelClassAttribute()
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
        public void TryValidateModelTestAJAX()
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
        public void TestBuddyValidation()
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
        public void TryValidateModelTestPage()
        {
            //TODO: Add a unit test for this
            // Not sure how this can be tested as it uses HttpContext
            // Perhaps with a refactor the form values can be extracted
            // and the functionality can be tested separately.
        }

        [TestMethod()]
        public void TestJSONObject()
        {
            JSONObject obj = new JSONObject();
            Assert.AreEqual(obj.Render(), "null");
            obj.Value = new JSONObjectEmpty();
            Assert.AreEqual(obj.Render(), "{}");
            obj.Attr("propertyName", "propertyValue");
            Assert.AreEqual(obj.Attr("propertyName").Value, "propertyValue");
            Assert.AreEqual(obj.Render(), "{\"propertyName\":\"propertyValue\"}");
            obj = new JSONObject(new { property1 = "value1", property2 = 5 });
            Assert.AreEqual(obj.Render(), "{\"property1\":\"value1\",\"property2\":5}");

        }

        [TestMethod()]
        public void TestHTMLTag()
        {
            HtmlTag tag = new HtmlTag("tagname", true);
            Assert.AreEqual(tag.Render(), "<tagname />");
            tag = new HtmlTag("tagname");
            Assert.AreEqual(tag.Render(), "<tagname></tagname>\r\n");
            tag.InnerText = "Sometext";
            Assert.AreEqual(tag.Render(), "<tagname>Sometext</tagname>\r\n");
            tag.Children.Add(new HtmlTag("tag2"));
            Assert.AreEqual(tag.Render(), "<tagname><tag2></tag2>\r\n</tagname>\r\n");
            tag.Attr("class", "someclass");
            Assert.AreEqual(tag.Render(), "<tagname class = \"someclass\"><tag2></tag2>\r\n</tagname>\r\n");
            tag.AddClass("newclass");
            Assert.AreEqual(tag.Render(), "<tagname class = \"someclass newclass\"><tag2></tag2>\r\n</tagname>\r\n");
            tag.RemoveClass("newclass");
            Assert.AreEqual(tag.Render(), "<tagname class = \"someclass\"><tag2></tag2>\r\n</tagname>\r\n");
            Assert.AreEqual(tag.Attr("class"), "someclass");

            Assert.AreEqual(tag.RenderBeginningOnly(), "<tagname class = \"someclass\">");
            Assert.AreEqual(tag.RenderEndingOnly(), "</tagname>\r\n");
            Assert.AreEqual(tag.IsClass("someclass"), true);

            tag.MergeObjectProperties(new { Class = "someotherclass", type = "tagtype" });
            Assert.AreEqual(tag.Render(), "<tagname class = \"someotherclass\" type = \"tagtype\"><tag2></tag2>\r\n</tagname>\r\n");
        }



        [TestMethod()]
        public void TestCompare()
        {
            int[] num1 = new int[] { 1, 2, 3 };
            int[] num2 = new int[] { 1, 2, 3 };
            AssertEqualReflection(num1, num2, null, true);

            int?[] num3 = new int?[] { null, new Int32?(2), new Int32?(3) };
            int?[] num4 = new int?[] { null, new Int32?(2), new Int32?(3) };
            AssertEqualReflection(num3, num4, null, true);

            DateTime?[] num5 = new DateTime?[] { null, DateTime.Parse("1/1/2001"), DateTime.Parse("1/1/2002") };
            DateTime?[] num6 = new DateTime?[] { null, DateTime.Parse("1/1/2001"), DateTime.Parse("1/1/2002") };
            AssertEqualReflection(num5, num6, null, true);

            object[] num7 = new object[] { 
                new { prop1 = "somevalue", prop2 = "somevalue2" }
            };
            object[] num8 = new object[] {
                new { prop1 = "somevalue", prop2 = "somevalue2" }
            };
            AssertEqualReflection(num7, num8, null, true);

        }

        public void AssertEqualReflection(object expected, object actual, string[] exclude)
        {
            AssertEqualReflection(expected, actual, exclude, false);
        }

        public void AssertEqualReflection(object expected, object actual, string[] exclude, bool enumerate)
        {
            if (expected == null && actual == null)
            { return; }
            Assert.AreEqual(expected.GetType(), actual.GetType());
            if (expected.GetType().IsPrimitive)
            {
                Assert.AreEqual(expected, actual);
                return;
            }

            if (enumerate && expected as IEnumerable != null)
            {
                List<object> index = new List<object>();
                foreach (var x in expected as IEnumerable)
                { index.Add(x); }
                int counter = 0;
                foreach (var x in actual as IEnumerable)
                {
                    if (counter > index.Count - 1) { Assert.Fail("expected count does not equal actual count."); }
                    AssertEqualReflection(x, index[counter], exclude, true); counter++;
                }
            }
            else
            {
                if (exclude == null) { exclude = new string[] { }; }
                foreach (PropertyInfo pi in expected.GetType().GetProperties().Where(p => !exclude.Contains(p.Name)))
                {
                    Assert.AreEqual(pi.GetValue(expected, null), pi.GetValue(actual, null), pi.Name);
                }
            }

        }

    }


    [MetadataType(typeof(BuddyClass))]
    public partial class FriendlyClass
    {
        public class BuddyClass
        {
            [Required]
            public string FirstName { get; set; }
            [StringLength(10)]
            public string LastName { get; set; }
            public string Address1 { get; set; }
        }
    }
    public partial class FriendlyClass
    {
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public string Address1 { get; set; } //No validation attributes on buddy
        public string City { get; set; } //Not present on buddy
    }

    public class TestWidget
    {
        [Required(ErrorMessage = "Widget name required")]
        public string RequiredName { get; set; }
        [StringLength(10, ErrorMessage="Max length 10 characters")]
        public string MaxLengthName { get; set; }
        [Range(5, 20, ErrorMessage = "Invalid number of sprockets.")]
        public int Sprockets { get; set; }
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$", ErrorMessage = "Widget must have valid e-mail")]
        public string Email { get; set; }
        [Price(ErrorMessage = "Invalid price")]
        public double Price { get; set; }
        [Required()]
        public string NoErrorMessage { get; set; }

        public String Password { get; set; }
        public String ConfirmPassword { get; set; }
    }

    [PropertiesMustMatch("Password", "ConfirmPassword")]
    public class ClassAttributeTestWidget
    {
        [Required(ErrorMessage = "Widget name required")]
        public string RequiredName { get; set; }
        [StringLength(10, ErrorMessage = "Max length 10 characters")]
        public string MaxLengthName { get; set; }
        [Range(5, 20, ErrorMessage = "Invalid number of sprockets.")]
        public int Sprockets { get; set; }
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$", ErrorMessage = "Widget must have valid e-mail")]
        public string Email { get; set; }
        [Price(ErrorMessage = "Invalid price")]
        public double Price { get; set; }
        [Required()]
        public string NoErrorMessage { get; set; }

        public String Password { get; set; }
        public String ConfirmPassword { get; set; }
    }

    public class PropertiesMustMatchAttribute : ValidationAttribute, IWFClientValidatable
    {
        public String FirstPropertyName { get; set; }
        public String SecondPropertyName { get; set; }

        public PropertiesMustMatchAttribute() {
        }

        public PropertiesMustMatchAttribute(String firstPropertyName, string secondPropertyName)
        {
            FirstPropertyName = firstPropertyName;
            SecondPropertyName = secondPropertyName;
        }
        public override bool IsValid(object value)
        {
            Type objectType = value.GetType();
            PropertyInfo[] props = objectType.GetProperties().Where(p => p.Name == FirstPropertyName ||
                p.Name == SecondPropertyName).ToArray();
            if (props.Count() != 2)
            {
                ErrorMessage = "Invalid property names or could not find properties on object " + objectType.Name;
                return false;
            }
            if (props[0].GetValue(value, null).ToString()
                .Equals(props[1].GetValue(value, null).ToString()))
            {
                return true;
            }
            //Could derive from displaynameattribute, too
            if (String.IsNullOrEmpty(ErrorMessage)) {
                ErrorMessage = props[0].Name + " and " + props[1].Name + " don't match.";
            } 
            
            return false;
        }

        #region IWFClientValidatable Members

        public IEnumerable<WFModelClientValidationRule> GetClientValidationRules() {
            var rule = new WFModelClientValidationRule
            {
                ErrorMessage = ErrorMessage,
                ValidationType = "propsmatch"
            };
            rule.ValidationParameters.Add("firstProp", FirstPropertyName);
            rule.ValidationParameters.Add("secondProp", SecondPropertyName);
            return new[] { rule };
        }

        public WFModelMetaProperty MetaProperty {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        #endregion
    }

    public class PriceAttribute : ValidationAttribute, IWFClientValidatable
    {
        public double MinPrice { get; set; }
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }
            var price = Double.Parse(value.ToString());
            if (price < MinPrice)
            {
                return false;
            }
            double cents = price - Math.Truncate(price);
            if (cents < 0.99 || cents >= 0.995)
            {
                return false;
            }

            return true;
        }

        #region IWFClientValidatable Members

        public IEnumerable<WFModelClientValidationRule> GetClientValidationRules() {
            var rule = new WFModelClientValidationRule {
                ErrorMessage = ErrorMessage,
                ValidationType = "price"
            };
            rule.ValidationParameters.Add("min", MinPrice);

            return new[] { rule };
        }

        private WFModelMetaProperty _MetaProperty = null;

        public WFModelMetaProperty MetaProperty {
            get { return _MetaProperty; }
            set { _MetaProperty = value; }
        }

        #endregion
    }

}
