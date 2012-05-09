using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebFormsUtilities.ValueProviders;
using WebFormsUtilities.Tests.TestObjects;

namespace WebFormsUtilities.Tests {
    /// <summary>
    /// Summary description for WFPageUtilitiesTest
    /// </summary>
    [TestClass]
    public class WFPageUtilitiesTest {

        #region WFPageUtilities.CallJSMethod()

        [TestMethod]
        public void CallJSMethodTest() {

            //TODO: HttpContext needed for this test
            Assert.Inconclusive();

            JSMethodTestPage testPage = new JSMethodTestPage();

            WFPageUtilities.CallJSMethod(testPage, null);

        }

        #endregion

        #region WFPageUtilities.EnableClientValidation()

        [TestMethod]
        public void EnableClientValidation() {
            WFPageUtilities.EnableClientValidation(new WFModelMetaData());
        }

        #endregion

        #region WFPageUtilities.ScriptRegisterClientFunctions()
        [TestMethod]
        public void ScriptRegisterClientFunctions() {
            WFPageUtilities.ScriptRegisterClientFunctions();
        }
        #endregion

        #region WFPageUtilities.TryValidateModel()

        [TestMethod]
        public void TryValidateModel() {
            //TODO: Write these tests
            Assert.Inconclusive();
        }

        #endregion

        #region WFPageUtilities.UpdateModel()

        [TestMethod]
        public void UpdateModel_WFDictionaryValueProvider() {
            Dictionary<string, string> valueDict = new Dictionary<string, string>();

            WFDictionaryValueProvider vp = new WFDictionaryValueProvider(valueDict);

            DestinationModel dm = new DestinationModel();

            WFPageUtilities.UpdateModel(vp, typeof(DestinationModel), "", null, null);


        }

        [TestMethod]
        public void UpdateModel_ValueConversionTests() {
            ConversionModel cm = new ConversionModel();
            WFObjectValueProvider provider = new WFObjectValueProvider(cm, "");
            DestinationModel dm = new DestinationModel();
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

        #endregion
    }
}
