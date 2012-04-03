using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebFormsUtilities.Examples.Functional.CustomValidation;

namespace WebFormsUtilities.Examples.Classes {
    public class Customer {
        [DisplayName("FirstName")]
        [Required]
        [StringLength(10)]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required]
        [StringLength(10)]
        public string LastName { get; set; }

        [RegularExpression(@"^((?!000)([0-6]\d{2}|[0-7]{2}[0-2]))-((?!00)\d{2})-((?!0000)\d{4})$"
            ,ErrorMessage="Must be a valid SSN (xxx-xx-xxxx)")]
        public string SocialSecurityNumber { get; set; }

        [Required]
        [ValidEmail]
        public string Email { get; set; }

        //This must be a value that ends in .99
        [DisplayName("Price")]
        [Required]
        [Price(ErrorMessage = "Invalid price")]
        public double Price { get; set; }

        public void Save() {
            //Just a sample
        }

        public static Customer CreateEmptyMockup() {
            Customer c = new Customer();
            c.Address = new CustomerAddress();
            c.Login = new CustomerLogin();
            return c;
        }

        public CustomerAddress Address { get; set; }

        public CustomerLogin Login { get; set; }
    }

    public class CustomerAddress {
        [Required]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
    }

    public class CustomerLogin {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [PasswordMustMatch]
        public string ConfirmPassword { get; set; }

    }


    public class StateUtility {
        /// <summary>
        /// Provide some sample states
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetStateList(string selectedState) {
            List<SelectListItem> states = new List<SelectListItem>();
            states.Add(new SelectListItem() {
                Selected = selectedState == "TX",
                Text = "Texas",
                Value = "TX"
            });
            states.Add(new SelectListItem() {
                Selected = selectedState == "MN",
                Text = "Minnesota",
                Value = "MN"
            });
            states.Add(new SelectListItem() {
                Selected = selectedState == "CA",
                Text = "California",
                Value = "CA"
            });
            return states;
        }
    }
}
