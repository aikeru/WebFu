using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel.DataAnnotations;
using WebFormsUtilities.ValueProviders;
using WebFormsUtilities.Tests.TestObjects;
using WebFormsUtilities.WebControls;

namespace WebFormsUtilities.Tests
{
    /// <summary>
    /// Summary description for WebControlUtilitiesTest
    /// </summary>
    [TestClass]
    public class WebControlUtilitiesTest
    {
        #region WebControlUtilities.ApplyModelToPage()

        [TestMethod]
        public void ApplyModelToPage_TextBox() {
            TestParticipantClass testPart = new TestParticipantClass();
            WFObjectValueProvider provider = new WFObjectValueProvider(testPart, "");
            Page testPage = new Page();
            TextBox FirstName = new TextBox() {
                ID = "FirstName"
            };
            testPage.Controls.Add(FirstName);

            //Apply null value (since it is a property initial state should be null)
            WebControlUtilities.ApplyModelToPage(testPage, provider);

            Assert.AreEqual("", ((TextBox)testPage.FindControl("FirstName")).Text);

            testPart.FirstName = "John";

            WebControlUtilities.ApplyModelToPage(testPage, provider);

            Assert.AreEqual("John", ((TextBox)testPage.FindControl("FirstName")).Text);

            testPart.FirstName = "";

            WebControlUtilities.ApplyModelToPage(testPage, provider);

            Assert.AreEqual("", ((TextBox)testPage.FindControl("FirstName")).Text);

        }
        [TestMethod]
        public void ApplyModelToPage_ListBox() {

            //Test applying a string property to ListBox
            TestParticipantClass testPart = new TestParticipantClass();
            WFObjectValueProvider provider = new WFObjectValueProvider(testPart, "");
            Page testPage = new Page();
            ListBox FirstName = new ListBox() {
                ID = "FirstName"
            };
            testPage.Controls.Add(FirstName);
            
            //null property value to empty listbox
            WebControlUtilities.ApplyModelToPage(testPage, provider);

            //null property value to listbox with empty item, but other selected
            FirstName.Items.Add(new ListItem("John", "1") { Selected = true });
            FirstName.Items.Add(new ListItem("Mark", "2"));
            FirstName.Items.Add(new ListItem("Joe", "3"));
            FirstName.Items.Add(new ListItem("Select a Name", ""));

            WebControlUtilities.ApplyModelToPage(testPage, provider);

            Assert.AreEqual(3, FirstName.SelectedIndex);

            //specific value

            testPart.FirstName = "2"; //go by value, not by text

            WebControlUtilities.ApplyModelToPage(testPage, provider);

            Assert.AreEqual(1, FirstName.SelectedIndex);

            testPart.FirstName = "NonExistantValue";

            WebControlUtilities.ApplyModelToPage(testPage, provider);

            Assert.AreEqual(-1, FirstName.SelectedIndex);

            //test multiple values

            testPage.Controls.Remove(FirstName);

            testPart.LuckyNumbers = new int[] { 1, 3 };

            ListBox LuckyNumbers = new ListBox();
            LuckyNumbers.Items.Add(new ListItem("1", "1"));
            LuckyNumbers.Items.Add(new ListItem("2", "2"));
            LuckyNumbers.Items.Add(new ListItem("3", "3"));
            LuckyNumbers.Items.Add(new ListItem("4", "4"));
            LuckyNumbers.Items.Add(new ListItem("5", "5"));

            //multiple values while in single mode
            LuckyNumbers.SelectionMode = ListSelectionMode.Single;
            LuckyNumbers.ID = "LuckyNumbers";
            testPage.Controls.Add(LuckyNumbers);

            WebControlUtilities.ApplyModelToPage(testPage, provider);
            //because we are in single mode, the array shouldn't be enumerated
            //returns "System.Int32[]..."
            Assert.AreEqual(-1, LuckyNumbers.SelectedIndex);

            //now change to multiple mode. Both items in the array should be selected.
            //No other items should be selected.
            LuckyNumbers.SelectionMode = ListSelectionMode.Multiple;

            WebControlUtilities.ApplyModelToPage(testPage, provider);

            foreach(ListItem li in LuckyNumbers.Items) {
                if(!li.Selected && testPart.LuckyNumbers.Contains(int.Parse(li.Value))) {
                    Assert.Fail("Item " + li.Value + " should be selected.");
                } else if (li.Selected && !testPart.LuckyNumbers.Contains(int.Parse(li.Value))) {
                    Assert.Fail("Item " + li.Value + " should not be selected.");
                }
            }
            

        }
        [TestMethod]
        public void ApplyModelToPage_DropDownList() {

            //Test applying a string property to ListBox
            TestParticipantClass testPart = new TestParticipantClass();
            WFObjectValueProvider provider = new WFObjectValueProvider(testPart, "");
            Page testPage = new Page();
            DropDownList FirstName = new DropDownList() {
                ID = "FirstName"
            };
            testPage.Controls.Add(FirstName);

            //null property value to empty listbox
            WebControlUtilities.ApplyModelToPage(testPage, provider);

            //null property value to listbox with empty item, but other selected
            FirstName.Items.Add(new ListItem("John", "1") { Selected = true });
            FirstName.Items.Add(new ListItem("Mark", "2"));
            FirstName.Items.Add(new ListItem("Joe", "3"));
            FirstName.Items.Add(new ListItem("Select a Name", ""));

            WebControlUtilities.ApplyModelToPage(testPage, provider);

            Assert.AreEqual(3, FirstName.SelectedIndex);

            //specific value

            testPart.FirstName = "2"; //go by value, not by text

            WebControlUtilities.ApplyModelToPage(testPage, provider);

            Assert.AreEqual(1, FirstName.SelectedIndex);

            testPart.FirstName = "NonExistantValue";

            WebControlUtilities.ApplyModelToPage(testPage, provider);

            //different from listbox. This should have the first item in the list selected
            //because a dropdownlist even though set to "-1" cannot have "no item selected"

            Assert.AreEqual(0, FirstName.SelectedIndex);


        }
        [TestMethod]
        public void ApplyModelToPage_RadioButton() {

            TestParticipantClass testPart = new TestParticipantClass();
            WFObjectValueProvider provider = new WFObjectValueProvider(testPart, "");
            Page testPage = new Page();
            RadioButton AcceptedRules = new RadioButton() {
                ID = "AcceptedRules"
            };
            testPage.Controls.Add(AcceptedRules);

            WebControlUtilities.ApplyModelToPage(testPage, provider);

            Assert.IsFalse(AcceptedRules.Checked);

            testPart.AcceptedRules = false;
            WebControlUtilities.ApplyModelToPage(testPage, provider);

            Assert.IsFalse(AcceptedRules.Checked);

            testPart.AcceptedRules = true;
            WebControlUtilities.ApplyModelToPage(testPage, provider);

            Assert.IsTrue(AcceptedRules.Checked);

        }
        [TestMethod]
        public void ApplyModelTopage_CheckBox() {

            TestParticipantClass testPart = new TestParticipantClass();
            WFObjectValueProvider provider = new WFObjectValueProvider(testPart, "");
            Page testPage = new Page();
            CheckBox AcceptedRules = new CheckBox() {
                ID = "AcceptedRules"
            };
            testPage.Controls.Add(AcceptedRules);

            WebControlUtilities.ApplyModelToPage(testPage, provider);

            Assert.IsFalse(AcceptedRules.Checked);

            testPart.AcceptedRules = false;
            WebControlUtilities.ApplyModelToPage(testPage, provider);

            Assert.IsFalse(AcceptedRules.Checked);

            testPart.AcceptedRules = true;
            WebControlUtilities.ApplyModelToPage(testPage, provider);

            Assert.IsTrue(AcceptedRules.Checked);


        }
        [TestMethod]
        public void ApplyModelToPage_IWFControlValue() {

            TestParticipantClass testPart = new TestParticipantClass();
            WFObjectValueProvider provider = new WFObjectValueProvider(testPart, "");
            Page testPage = new Page();
            TestControlValueControl testControl = new TestControlValueControl() {
                ID = "FirstName"
            };
            testPage.Controls.Add(testControl);

            WebControlUtilities.ApplyModelToPage(testPage, provider);

            Assert.AreEqual("", testControl.ControlValue);

            testPart.FirstName = "Sam";

            WebControlUtilities.ApplyModelToPage(testPage, provider);

            Assert.AreEqual("Sam", testControl.ControlValue);

        }


        [TestMethod]
        public void ApplyModelToPage_MultipleTests()
        {
            TestParticipantClass testPart = new TestParticipantClass();

            testPart.FirstName = "John";
            testPart.LastName = "Doe";
            testPart.BirthDate = null;

            Page px = new Page();
            px.Controls.Add(new TextBox() { ID = "FirstName" });
            px.Controls.Add(new TextBox() { ID = "LastName" });
            px.Controls.Add(new TextBox() { ID = "BirthDate" });

            WebControlUtilities.ApplyModelToPage(px, testPart);

            TextBox txtFirstName = (TextBox)px.FindControl("FirstName");
            TextBox txtLastName = (TextBox)px.FindControl("LastName");
            TextBox txtBirthDate = (TextBox)px.FindControl("BirthDate");

            Assert.AreEqual(testPart.FirstName, txtFirstName.Text);
            Assert.AreEqual(testPart.LastName, txtLastName.Text);
            //BirthDate is null, so we expect blank
            Assert.AreEqual("", txtBirthDate.Text);

            txtFirstName.Text = "Jack";
            txtLastName.Text = "Sparrow";
            txtBirthDate.Text = "1/1/2001";

            TestParticipantClass part2 = new TestParticipantClass();
            WFPageControlsValueProvider valueProvider = new WFPageControlsValueProvider(px, "");
            WFPageUtilities.UpdateModel(valueProvider, part2, "", null, null);

            Assert.AreEqual("Jack", part2.FirstName);
            Assert.AreEqual("Sparrow", part2.LastName);
            Assert.AreEqual(DateTime.Parse("1/1/2001"), part2.BirthDate);
               
        }
        [TestMethod]
        public void ApplyModelToPage_ListBoxDropDownAdditionalTests()
        {
            ListBox lbItems = new ListBox() { ID = "Inventory" };
            lbItems.Items.Add(new ListItem() { Text = "Item1", Value = "1" });
            lbItems.Items.Add(new ListItem() { Text = "Item2", Value = "2" });
            lbItems.Items.Add(new ListItem() { Text = "Item3", Value = "3" });
            lbItems.Items.Add(new ListItem() { Text = "Item4", Value = "4" });

            DropDownList ddlItems = new DropDownList() { ID = "LuckyPhrases" };
            ddlItems.Items.Add(new ListItem() { Text = "Phrase1", Value = "A lucky man" });
            ddlItems.Items.Add(new ListItem() { Text = "Phrase2", Value = "Good fortune" });
            ddlItems.Items.Add(new ListItem() { Text = "Phrase3", Value = "Rich and wealthy" });
            ddlItems.Items.Add(new ListItem() { Text = "Phrase4", Value = "Excellent health" });

            ListBox lbGroups = new ListBox() { ID = "ParticipantGroupID" };
            lbGroups.Items.Add(new ListItem() { Text = "Admin", Value = "1" });
            lbGroups.Items.Add(new ListItem() { Text = "Judge", Value = "2" });
            lbGroups.Items.Add(new ListItem() { Text = "User", Value = "3" });
            lbGroups.Items.Add(new ListItem() { Text = "Outsider", Value = "4" });

            Page px = new Page();
            //px.Controls.Add(lbItems);
            //px.Controls.Add(ddlItems);
            px.Controls.Add(lbGroups);

            lbGroups.Items[2].Selected = true;

            //Pull selected values into participant
            TestParticipantClass testPart = new TestParticipantClass();
            WFPageControlsValueProvider valueProvider = new WFPageControlsValueProvider(px, "");
            WFPageUtilities.UpdateModel(valueProvider, testPart, "", null, null);

            Assert.AreEqual(3, testPart.ParticipantGroupID);


            //Apply null values to null collections
            WebControlUtilities.ApplyModelToPage(px, testPart);

            Assert.IsNull(testPart.LuckyPhrases, null);
            Assert.IsNull(testPart.Inventory, null);

            //Apply null values to non-null collections
            testPart.LuckyPhrases = new string[] { "A lucky man", "Rich and wealthy" };
            testPart.Inventory = new List<TestInventoryObject>();
            WebControlUtilities.ApplyModelToPage(px, testPart);

            Assert.IsNotNull(testPart.Inventory);
            Assert.AreEqual(0, testPart.Inventory.Count);
            Assert.AreEqual(2, testPart.LuckyPhrases.Length);

            //Apply select one value
            lbItems.Items[2].Selected = true;
            ddlItems.Items[2].Selected = true;

            WebControlUtilities.ApplyModelToPage(px, testPart);


        }
        #endregion

        #region WebControlUtilities.FindControlRecursive()

        [TestMethod]
        public void FindControlRecursiveTest() {
            Page p = new Page();
            TextBox testControl = new TextBox() { ID = "testControl1" };
            TextBox testControl2 = new TextBox() { ID = "testControl2" };
            TextBox testControl3 = new TextBox() { ID = "testControl3" };
            TextBox testControl4 = new TextBox() { ID = "testControl4" };

            p.Controls.Add(testControl);
            Panel panel1 = new Panel();
            Panel panel2 = new Panel();

            panel1.Controls.Add(testControl2);
            panel2.Controls.Add(testControl3);

            p.Controls.Add(panel1);
            p.Controls.Add(panel2);

            Assert.IsNotNull(WebControlUtilities.FindControlRecursive(p, "testControl1"));
            Assert.IsNotNull(WebControlUtilities.FindControlRecursive(p, "testControl2"));
            Assert.IsNotNull(WebControlUtilities.FindControlRecursive(p, "testControl3"));

            Assert.IsNull(WebControlUtilities.FindControlRecursive(panel1, "testControl1"));
            Assert.IsNotNull(WebControlUtilities.FindControlRecursive(panel1, "testControl2"));

        }

        #endregion

        #region WebControlUtilities.FindValidators()

        [TestMethod]
        public void FindValidatorsTest() {
            Page p = new Page();
            DataAnnotationValidatorControl testControl = new DataAnnotationValidatorControl() { ID = "testControl1" };
            DataAnnotationValidatorControl testControl2 = new DataAnnotationValidatorControl() { ID = "testControl2" };
            DataAnnotationValidatorControl testControl3 = new DataAnnotationValidatorControl() { ID = "testControl3" };
            DataAnnotationValidatorControl testControl4 = new DataAnnotationValidatorControl() { ID = "testControl4" };

            p.Controls.Add(testControl);
            Panel panel1 = new Panel();
            Panel panel2 = new Panel();

            panel1.Controls.Add(testControl2);
            panel2.Controls.Add(testControl3);

            p.Controls.Add(panel1);
            p.Controls.Add(panel2);

            Assert.IsNotNull(WebControlUtilities.FindControlRecursive(p, "testControl1"));
            Assert.IsNotNull(WebControlUtilities.FindControlRecursive(p, "testControl2"));
            Assert.IsNotNull(WebControlUtilities.FindControlRecursive(p, "testControl3"));

            Assert.IsNull(WebControlUtilities.FindControlRecursive(panel1, "testControl1"));
            Assert.IsNotNull(WebControlUtilities.FindControlRecursive(panel1, "testControl2"));
        }

        #endregion

        //TODO: Write these tests

        #region WebControlUtilities.FlattenPageControls()
        #endregion
        #region WebControlUtilities.GetControlValue()
        #endregion
        #region WebControlUtilities.GetMetaPropertyFromValidator()
        #endregion
    }
}
