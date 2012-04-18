using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebFormsUtilities.Tests.TestObjects {
    public class ConversionModel {
        public string stringToShort { get; set; } //= "10";
        public string stringToInt { get; set; }// = "10";
        public string stringToLong { get; set; } //= "10";
        public string stringToDecimal { get; set; }// = "10.1";
        public string stringToFloat { get; set; }// = "10.2";
        public int intToShort { get; set; }//= 10;
        public int intToInt { get; set; }//= 10;
        public int intToLong { get; set; }//= 10;
        public int intToDecimal { get; set; } //= 10;
        public int intToFloat { get; set; } //= 10;

        public object nullToInt { get; set; }
        public object nullToFloat { get; set; }
        public object nullToDateTime { get; set; }

        public string blankToInt { get; set; }
        public string blankToFloat { get; set; }
        public string blankToDateTime { get; set; }

        public DateTime dateToString { get; set; } //= DateTime.Parse("1/1/2001");
        public string stringToDate { get; set; }//= "1/1/2002";

        public string stringToNullableInt { get; set; }
        public string stringToNullableDouble { get; set; }
        public string stringToNullableDateTime { get; set; }
        public string stringToNullableBoolean { get; set; }

        public ConversionModel() {
            stringToShort = "10";
            stringToInt = "10";
            stringToLong = "10";
            stringToDecimal = "10.1";
            stringToFloat = "10.2";
            intToShort = 10;
            intToInt = 10;
            intToLong = 10;
            intToDecimal = 10;
            intToFloat = 10;
            dateToString = DateTime.Parse("1/1/2001");
            stringToDate = "1/1/2002";
        }

    }
    public class DestinationModel {

        public short stringToShort { get; set; }
        public int stringToInt { get; set; }
        public long stringToLong { get; set; }
        public decimal stringToDecimal { get; set; }
        public float stringToFloat { get; set; }

        public short intToShort { get; set; }
        public int intToInt { get; set; }
        public long intToLong { get; set; }
        public decimal intToDecimal { get; set; }
        public float intToFloat { get; set; }

        public int nullToInt { get; set; }
        public float nullToFloat { get; set; }
        public DateTime nullToDateTime { get; set; }

        public int blankToInt { get; set; }
        public float blankToFloat { get; set; }
        public DateTime blankToDateTime { get; set; }

        public string dateToString { get; set; }
        public DateTime stringToDate { get; set; }

        public Int32? stringToNullableInt { get; set; }
        public Double? stringToNullableDouble { get; set; }
        public DateTime? stringToNullableDateTime { get; set; }
        public Boolean? stringToNullableBoolean { get; set; }
    }
}
