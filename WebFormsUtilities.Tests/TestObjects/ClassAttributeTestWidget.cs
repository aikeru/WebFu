using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace WebFormsUtilities.Tests.TestObjects {
    [PropertiesMustMatch("Password", "ConfirmPassword")]
    public class ClassAttributeTestWidget {
        [Required(ErrorMessage = "Widget name required")]
        public string RequiredName { get; set; }
        [StringLength(10, ErrorMessage = "Max length 10 characters")]
        public string MaxLengthName { get; set; }
        [Range(5, 20, ErrorMessage = "Invalid number of sprockets.")]
        public int Sprockets { get; set; }
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$", ErrorMessage = "Widget must have valid e-mail")]
        public string Email { get; set; }
        [Price(ErrorMessage = "Invalid price")]
        public double Price { get; set; }
        [Required()]
        public string NoErrorMessage { get; set; }

        public String Password { get; set; }
        public String ConfirmPassword { get; set; }
    }
}
