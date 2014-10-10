using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeoKmlLibrary.Kml;
using GeoKmlLibrary.Kml.Geometry;
using System.Xml.Linq;
using FluentAssertions;
namespace GeoKmlLibrary.UnitTests.Kml
{
    [TestClass]
    public class FeatureTest
    {
        [TestMethod]
        public void ToKml_Is_Correct()
        {
            var feature = new Feature();
            feature.Description = "Mannus";
            feature.Geometry = new Point(10.1, 10.3);
            feature.Name = "Stefania";
            feature.SymbolName = "Test";
            XElement result = feature.ToKml();
            result.Should().HaveElement("name").And
                .HaveElement("Point");
        }
    }
}
