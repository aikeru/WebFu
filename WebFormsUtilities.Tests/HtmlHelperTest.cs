using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebFormsUtilities.Tests.TestObjects;

namespace WebFormsUtilities.Tests {
    /// <summary>
    /// Summary description for HtmlHelperTest
    /// </summary>
    [TestClass]
    public class HtmlHelperTest : System.Web.UI.Page, IWebFormsView<TestParticipantClass> {

        HtmlHelper<TestParticipantClass> _Html = null;

        [TestMethod]
        public void AddPreProcessor() {
            Assert.Inconclusive();
        }
        [TestMethod]
        public void CheckBox() {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void CheckBoxFor() {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ClearPreProcessors() {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void DropDownList() {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void DropDownListFor() {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void LabelFor() {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void RadioButton() {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void RadioButtonFor() {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void RenderControl() {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void TextArea() {
            Assert.Inconclusive();
        }
        [TestMethod]
        public void TextAreaFor() {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void TextBox() {
            Assert.Inconclusive();
        }
        [TestMethod]
        public void TextBoxFor() {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ValidationMessageFor() {
            Assert.Inconclusive();
        }
        [TestMethod]
        public void ValidationSummary() {
            Assert.Inconclusive();
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
