using WebFormsUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.Expressions;
using WebFormsUtilities.Tests.TestObjects;

namespace WebFormsUtilities.Tests
{
    
    
    /// <summary>
    ///This is a test class for ModelMetaDataTest and is intended
    ///to contain all ModelMetaDataTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ModelMetaDataTest {

        #region ModelMetaData.FromLambdaExpression
        [TestMethod()]
        public void FromLambdaExpression_StringProperty() {
            TestParticipantClass tpc = new TestParticipantClass();
            tpc.FirstName = "Michael";
            Expression<Func<TestParticipantClass, String>> expression = p => p.FirstName;

            ModelMetaData mmd = ModelMetaData.FromLambdaExpression(expression, tpc);
            Assert.AreEqual("FirstName", mmd.PropertyName);
            Assert.AreEqual(false, mmd.IsSelf);
            Assert.AreEqual(tpc.FirstName, mmd.ModelAccessor());
            
        }
        [TestMethod()]
        public void FromLambdaExpression_ModelSelfTest() {
            TestParticipantClass tpc = new TestParticipantClass();
            Expression<Func<TestParticipantClass, TestParticipantClass>> expression = p => p;

            ModelMetaData mmd = ModelMetaData.FromLambdaExpression(expression, tpc);
            Assert.AreEqual("", mmd.PropertyName);
            Assert.AreEqual(true, mmd.IsSelf);
            Assert.AreEqual(tpc, mmd.ModelAccessor());

        }
        [TestMethod()]
        public void FromLambdaExpression_ChildObject() {
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
        public void FromLambdaExpression_ChildObjectProperty() {
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
        #endregion
    }
}
