using WebFormsUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.Expressions;

namespace WebFormsUtilities.Tests
{
    
    
    /// <summary>
    ///This is a test class for ModelMetaDataTest and is intended
    ///to contain all ModelMetaDataTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ModelMetaDataTest {


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
        public void TestFromLambdaExpressionStringProperty() {
            TestParticipantClass tpc = new TestParticipantClass();
            tpc.FirstName = "Michael";
            Expression<Func<TestParticipantClass, String>> expression = p => p.FirstName;

            ModelMetaData mmd = ModelMetaData.FromLambdaExpression(expression, tpc);
            Assert.AreEqual("FirstName", mmd.PropertyName);
            Assert.AreEqual(false, mmd.IsSelf);
            Assert.AreEqual(tpc.FirstName, mmd.ModelAccessor());
            
        }
        [TestMethod()]
        public void TestFromLambdaExpressionModel() {
            TestParticipantClass tpc = new TestParticipantClass();
            Expression<Func<TestParticipantClass, TestParticipantClass>> expression = p => p;

            ModelMetaData mmd = ModelMetaData.FromLambdaExpression(expression, tpc);
            Assert.AreEqual("", mmd.PropertyName);
            Assert.AreEqual(true, mmd.IsSelf);
            Assert.AreEqual(tpc, mmd.ModelAccessor());

        }
        [TestMethod()]
        public void TestFromLambdaExpressionChildObject() {
            TestParticipantClass tpc = new TestParticipantClass();
            Expression<Func<TestParticipantClass, TestParticipantAddressClass>> expression = p => p.Address;

            ModelMetaData mmd = ModelMetaData.FromLambdaExpression(expression, tpc);
            Assert.AreEqual("Address", mmd.PropertyName);
            Assert.AreEqual(false, mmd.IsSelf);
            Assert.AreEqual(tpc.Address, mmd.ModelAccessor());

            tpc.Address = new TestParticipantAddressClass();
            mmd = ModelMetaData.FromLambdaExpression(expression, tpc);
            Assert.AreEqual("Address", mmd.PropertyName);
            Assert.AreEqual(false, mmd.IsSelf);
            Assert.AreEqual(tpc.Address, mmd.ModelAccessor());
        }

        [TestMethod()]
        public void TestFromLambdaExpressionChildObjectProperty() {
            TestParticipantClass tpc = new TestParticipantClass();
            tpc.Address = new TestParticipantAddressClass();
            Expression<Func<TestParticipantClass, string>> expression = p => p.Address.Address1;

            ModelMetaData mmd = ModelMetaData.FromLambdaExpression(expression, tpc);
            Assert.AreEqual("Address1", mmd.PropertyName);
            Assert.AreEqual(false, mmd.IsSelf);
            Assert.AreEqual(tpc.Address.Address1, mmd.ModelAccessor());


            tpc.Address.Address1 = "101 someplace";
            mmd = ModelMetaData.FromLambdaExpression(expression, tpc);
            Assert.AreEqual("Address1", mmd.PropertyName);
            Assert.AreEqual(false, mmd.IsSelf);
            Assert.AreEqual(tpc.Address.Address1, mmd.ModelAccessor());
        }
    }
}
