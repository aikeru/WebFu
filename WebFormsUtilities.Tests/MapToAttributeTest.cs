using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebFormsUtilities.ValueProviders;

namespace WebFormsUtilities.Tests {
    /// <summary>
    /// Summary description for MapToAttributeTest
    /// </summary>
    [TestClass]
    public class MapToAttributeTest {

        [TestMethod]
        public void MapToAttribute_MappingTests() {
            SourceMap sm = new SourceMap();
            sm.Credits = 500;
            sm.DateOfBirth = DateTime.Parse("1/1/2001");
            sm.FirstName = "Samwise";
            sm.LastName = "Gamgee";
            sm.MyAddress = "110 someplace";

            SourceMapDestination smd = new SourceMapDestination();
            smd.Address1 = "220 otherplace";
            WFMapToAttributeValueProvider mapper = 
                new WFMapToAttributeValueProvider(typeof(SourceMapProxy), sm);
            WFPageUtilities.UpdateModel(mapper, smd, "", null, new string[] { "Address1" });

            Assert.AreEqual(sm.Credits, smd.Points);
            Assert.AreEqual(sm.Credits, smd.TotalPoints);
            Assert.AreEqual(sm.FirstName, smd.FirstName);
            Assert.AreEqual(sm.LastName, smd.LastName);
            Assert.AreEqual(sm.DateOfBirth, smd.Birthday);
            Assert.AreEqual(sm.DateOfBirth, smd.DateBorn);
            Assert.AreNotEqual(sm.MyAddress, smd.Address1);

        }
    }

    public class SourceMap {
        public int Credits { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MyAddress { get; set; }
    }

    public class SourceMapProxy {
        [MapTo(new string[] { "Points", "TotalPoints" })]
        public int Credits { get; set; }

        [MapTo("Birthday")]
        [MapTo("DateBorn")]
        public DateTime DateOfBirth { get; set; }

        [MapTo("FirstName")]
        public string FirstName { get; set; }

        [MapTo("LastName")]
        public string LastName { get; set; }

        [MapTo("Address1")] 
        public string MyAddress { get; set; }
    }
    public class SourceMapDestination {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime DateBorn { get; set; }
        public int Points { get; set; }
        public int TotalPoints { get; set; }
    }
}
