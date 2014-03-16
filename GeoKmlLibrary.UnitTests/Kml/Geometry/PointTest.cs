using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using GeoKmlLibrary.Kml;
using System.Xml.Linq;
using GeoKmlLibrary.Kml.Geometry;
namespace GeoKmlLibrary.UnitTests.Kml.Geometry
{
    [TestClass]
    public class PointTest
    {
        [TestMethod]
        public void ToXml_Is_Correct()
        {
            Point p = new Point();
            p.Longitude = 6.57869428396225;
            p.Latitude = 53.1983207520695;
            XElement xml = p.ToKml();
            xml.Should().HaveElement("coordinates");
        }
    }
}