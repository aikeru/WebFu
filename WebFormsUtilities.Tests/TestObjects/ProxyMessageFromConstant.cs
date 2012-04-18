using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace WebFormsUtilities.Tests.TestObjects {
 
    public class ProxyMessageFromConstant {
        [Required(ErrorMessage = "This is a constant error.")]
        public string FirstName { get; set; }
    }
}
