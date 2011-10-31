using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFormsUtilities.Tests
{
    /// <summary>
    /// Summary description for WebControlUtilitiesTest
    /// </summary>
    [TestClass]
    public class WebControlUtilitiesTest
    {
        public WebControlUtilitiesTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

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

        //[TestMethod]
        //public void TestMethod1()
        //{
        //    //
        //    // TODO: Add test logic	here
        //    //
        //}

        [TestMethod]
        public void TestApplyPageValues()
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
        public void TestApplyListBoxDropDown()
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

    }

    public class TestParticipantClass
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Points { get; set; }
        public int ParticipantGroupID { get; set; }
        public List<TestInventoryObject> Inventory { get; set; }
        public TestParticipantAddressClass Address { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime LastModified { get; set; }
        public DateTime DateCreated { get; set; }
        public int[] LuckyNumbers { get; set; }
        public string[] LuckyPhrases { get; set; }
        public DateTime? NullDate1 { get; set; }
        public DateTime? NullDate2 { get; set; }
        public DateTime? NullDate3 { get; set; }
    }
    public class TestParticipantAddressClass
    {
        public string Address1 { get; set; }
        public string City { get; set; }
        public int PhoneNumber { get; set; }
    }
    public class TestInventoryObject
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
