using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebFormsUtilities.Tests.TestObjects {
    public class TestParticipantClass {
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

        public decimal Price { get; set; }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public bool AcceptedRules { get; set; }
    }

}
