using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebFormsUtilities.ValueProviders;

namespace WebFormsUtilities.Tests {
    /// <summary>
    /// Summary description for TestConversion
    /// </summary>
    [TestClass]
    public class TestConversion {
        public TestConversion() {
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
        public void TestUpdateConversion() {
            ConversionModel cm = new ConversionModel();
            WFObjectValueProvider provider = new WFObjectValueProvider(cm, "");
            DestinationModel dm  =new DestinationModel();
            dm.stringToNullableBoolean = true;
            dm.stringToNullableDateTime = DateTime.Parse("1/1/2001");
            dm.stringToNullableDouble = 0.5d;
            dm.stringToNullableInt = 5;

            WFPageUtilities.UpdateModel(provider, dm, "", null, null);

            Assert.AreEqual(10, dm.stringToShort);
            Assert.AreEqual(10, dm.stringToInt);
            Assert.AreEqual(10, dm.stringToLong);
            Assert.AreEqual(10.1m, dm.stringToDecimal);
            Assert.AreEqual(10.2f, dm.stringToFloat);
            Assert.AreEqual(10, dm.intToShort);
            Assert.AreEqual(10, dm.intToLong);
            Assert.AreEqual(10, dm.intToFloat);
            Assert.AreEqual(default(DateTime), dm.blankToDateTime);
            Assert.AreEqual(default(DateTime), dm.nullToDateTime);
            Assert.AreEqual(default(int), dm.nullToInt);
            Assert.AreEqual(default(int), dm.blankToInt);

            Assert.AreEqual(DateTime.Parse("1/1/2001").ToString(), dm.dateToString);
            Assert.AreEqual(DateTime.Parse("1/1/2002"), dm.stringToDate);

            // Test null =================================
            Assert.IsNull(dm.stringToNullableBoolean);
            Assert.IsNull(dm.stringToNullableDateTime);
            Assert.IsNull(dm.stringToNullableDouble);
            Assert.IsNull(dm.stringToNullableInt);

            // Test "" ===================================
            cm.stringToNullableBoolean = "";
            cm.stringToNullableDateTime = "";
            cm.stringToNullableDouble = "";
            cm.stringToNullableInt = "";

            dm.stringToNullableBoolean = true;
            dm.stringToNullableDateTime = DateTime.Parse("1/1/2001");
            dm.stringToNullableDouble = 0.5d;
            dm.stringToNullableInt = 5;

            WFPageUtilities.UpdateModel(provider, dm, "", null, null);
            Assert.IsNull(dm.stringToNullableBoolean);
            Assert.IsNull(dm.stringToNullableDateTime);
            Assert.IsNull(dm.stringToNullableDouble);
            Assert.IsNull(dm.stringToNullableInt);

            // Test "null" ===============================
            cm.stringToNullableBoolean = "null";
            cm.stringToNullableDateTime = "null";
            cm.stringToNullableDouble = "null";
            cm.stringToNullableInt = "null";

            dm.stringToNullableBoolean = true;
            dm.stringToNullableDateTime = DateTime.Parse("1/1/2001");
            dm.stringToNullableDouble = 0.5d;
            dm.stringToNullableInt = 5;
            WFPageUtilities.UpdateModel(provider, dm, "", null, null);
            Assert.IsNull(dm.stringToNullableBoolean);
            Assert.IsNull(dm.stringToNullableDateTime);
            Assert.IsNull(dm.stringToNullableDouble);
            Assert.IsNull(dm.stringToNullableInt);

            // Test values (except bool)
            cm.stringToNullableDateTime = "1/1/2015";
            cm.stringToNullableDouble = "0.7";
            cm.stringToNullableInt = "77";

            dm.stringToNullableDateTime = DateTime.Parse("1/1/2001");
            dm.stringToNullableDouble = 0.5d;
            dm.stringToNullableInt = 5;
            WFPageUtilities.UpdateModel(provider, dm, "", null, null);
            Assert.IsTrue(dm.stringToNullableDateTime.HasValue);
            Assert.IsTrue(dm.stringToNullableDouble.HasValue);
            Assert.IsTrue(dm.stringToNullableInt.HasValue);
            Assert.AreEqual(DateTime.Parse("1/1/2015"), dm.stringToNullableDateTime.Value);
            Assert.AreEqual(Double.Parse("0.7"), dm.stringToNullableDouble.Value);
            Assert.AreEqual(Int32.Parse("77"), dm.stringToNullableInt.Value);

            string[] trueValues = { "true", "true,false", "on" };
            foreach (string s in trueValues) {
                // truthy values test
                cm.stringToNullableBoolean = s;
                dm.stringToNullableBoolean = null;
                WFPageUtilities.UpdateModel(provider, dm, "", null, null);
                Assert.IsTrue(dm.stringToNullableBoolean.HasValue && dm.stringToNullableBoolean.Value);
            }

        }

    }

    public class ConversionModel {
        public string stringToShort { get; set; } //= "10";
        public string stringToInt { get; set; }// = "10";
        public string stringToLong { get; set; } //= "10";
        public string stringToDecimal { get; set; }// = "10.1";
        public string stringToFloat { get; set; }// = "10.2";
        public int intToShort  { get; set; }//= 10;
        public int intToInt { get; set; }//= 10;
        public int intToLong  { get; set; }//= 10;
        public int intToDecimal { get; set; } //= 10;
        public int intToFloat { get; set; } //= 10;

        public object nullToInt { get; set; }
        public object nullToFloat { get; set; }
        public object nullToDateTime { get; set; }

        public string blankToInt { get; set; }
        public string blankToFloat { get; set; }
        public string blankToDateTime { get; set; }
        
        public DateTime dateToString { get; set; } //= DateTime.Parse("1/1/2001");
        public string stringToDate { get; set; }//= "1/1/2002";

        public string stringToNullableInt { get; set; }
        public string stringToNullableDouble { get; set; }
        public string stringToNullableDateTime { get; set; }
        public string stringToNullableBoolean { get; set; }

        public ConversionModel() {
            stringToShort = "10";
            stringToInt = "10";
            stringToLong = "10";
            stringToDecimal = "10.1";
            stringToFloat = "10.2";
            intToShort = 10;
            intToInt = 10;
            intToLong = 10;
            intToDecimal = 10;
            intToFloat = 10;
            dateToString = DateTime.Parse("1/1/2001");
            stringToDate = "1/1/2002";
        }

    }
    public class DestinationModel {

        public short stringToShort { get; set; }
        public int stringToInt { get; set; }
        public long stringToLong { get; set; }
        public decimal stringToDecimal { get; set; }
        public float stringToFloat { get; set; }

        public short intToShort { get; set; }
        public int intToInt { get; set; }
        public long intToLong { get; set; }
        public decimal intToDecimal { get; set; }
        public float intToFloat { get; set; }

        public int nullToInt { get; set; }
        public float nullToFloat { get; set; }
        public DateTime nullToDateTime { get; set; }

        public int blankToInt { get; set; }
        public float blankToFloat { get; set; }
        public DateTime blankToDateTime { get; set; }

        public string dateToString { get; set; }
        public DateTime stringToDate { get; set; }

        public Int32? stringToNullableInt { get; set; }
        public Double? stringToNullableDouble { get; set; }
        public DateTime? stringToNullableDateTime { get; set; }
        public Boolean? stringToNullableBoolean { get; set; }
    }
}
