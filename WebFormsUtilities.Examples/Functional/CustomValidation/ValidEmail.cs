using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebFormsUtilities.Examples.Functional.CustomValidation {
    public class ValidEmailAttribute : RegularExpressionAttribute {
        public ValidEmailAttribute()
            : base(ValidEmailAttribute.RegexPattern) {
        }

        public static string RegexPattern {
            get {
                string qtext = "[^\\x0d\\x22\\x5c\\x80-\\xff]"; // <any CHAR excepting <">, "\" & CR, and including linear-white-space>
                string dtext = "[^\\x0d\\x5b-\\x5d\\x80-\\xff]"; // <any CHAR excluding "[", "]", "\" & CR, & including linear-white-space>
                string atom = "[^\\x00-\\x20\\x22\\x28\\x29\\x2c\\x2e\\x3a-\\x3c\\x3e\\x40\\x5b-\\x5d\\x7f-\\xff]+"; // *<any CHAR except specials, SPACE and CTLs>
                string quoted_pair = "\\x5c[\\x00-\\x7f]"; // "\" CHAR 
                string quoted_string = string.Format("\\x22({0}|{1})*\\x22", qtext, quoted_pair); // <"> *(qtext/quoted-pair) <">
                string word = string.Format("({0}|{1})", atom, quoted_string); //atom / quoted-string
                string domain_literal = string.Format("\\x5b({0}|{1})*\\x5d", dtext, quoted_pair); // "[" *(dtext / quoted-pair) "]"

                string domain_ref = atom; // atom 
                string sub_domain = string.Format("({0}|{1})", domain_ref, domain_literal); // domain-ref / domain-literal
                string domain = string.Format("{0}(\\x2e{0})*", sub_domain); // sub-domain *("." sub-domain)
                string local_part = string.Format("{0}(\\x2e{0})*", word); // word *("." word) 
                string addr_spec = string.Format("{0}\\x40{1}", local_part, domain); //local-part "@" domain

                string regexPattern = string.Format("^{0}$", addr_spec);

                return regexPattern;
            }
        }
    }
}
