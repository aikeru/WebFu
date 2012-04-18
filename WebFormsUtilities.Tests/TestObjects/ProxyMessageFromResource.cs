using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace WebFormsUtilities.Tests.TestObjects {

    public class ProxyMessageFromResource {
        [Required(ErrorMessageResourceName = "FirstName_ErrorMessage_Test1", ErrorMessageResourceType = typeof(WebFormsUtilities.Tests.Properties.Resources))]
        public string FirstName { get; set; }
    }
}
