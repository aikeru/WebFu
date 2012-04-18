using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace WebFormsUtilities.Tests.TestObjects {
    public class ProxyMessageFromValidator {
        [Required]
        public string FirstName { get; set; }
    }
}
