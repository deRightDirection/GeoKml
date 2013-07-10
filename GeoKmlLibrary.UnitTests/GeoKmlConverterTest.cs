using System;
using System.Collections.Generic;
using System.Data.Spatial;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeoKmlLibrary.UnitTests
{
    [TestClass]
    public class GeoKmlConverterTest
    {
        [TestMethod]
        public void ConvertToKml()
        {
            var testObject = new TestObject() { TestAsDescription = "Description", TestAsTitle = "Title", TestAsGeometry = GeoUtils.CreatePoint(10, 5) };
 GeoKmlConverter converter = new GeoKmlConverter();
     var xml = converter.ConvertToGeoKml<TestObject>(testObject);
     StringAssert.StartsWith(xml, "<?xml");
 
        }

        [TestMethod]
        public void ConvertListToKml()
        {
            var testObject = new TestObject() { TestAsDescription = "Description", TestAsTitle = "Title", TestAsGeometry = GeoUtils.CreatePoint(10, 5) };
            var testObject2 = new TestObject() { TestAsDescription = "Description2", TestAsTitle = "Title2", TestAsGeometry = GeoUtils.CreatePoint(10, 5) };
            List<TestObject> objects = new List<TestObject>() { testObject, testObject2};
            GeoKmlConverter converter = new GeoKmlConverter();
            var xml = converter.ConvertToGeoKml<List<TestObject>>(objects);
            StringAssert.StartsWith(xml, "<?xml");
        }
    }
}
