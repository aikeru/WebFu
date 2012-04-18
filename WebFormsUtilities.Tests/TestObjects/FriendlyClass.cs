using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace WebFormsUtilities.Tests.TestObjects {
    [MetadataType(typeof(BuddyClass))]
    public partial class FriendlyClass {
        public class BuddyClass {
            [Required]
            public string FirstName { get; set; }
            [StringLength(10)]
            public string LastName { get; set; }
            public string Address1 { get; set; }
        }
    }
    public partial class FriendlyClass {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; } //No validation attributes on buddy
        public string City { get; set; } //Not present on buddy
    }

}
