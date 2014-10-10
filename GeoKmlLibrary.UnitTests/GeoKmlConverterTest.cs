using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using FluentAssertions;
using GeoKmlLibrary.Kml.Symbol;
namespace GeoKmlLibrary.UnitTests
{
    [TestClass]
    public class GeoKmlConverterTest
    {
        private GeoKmlConverter _kmlConverter;

        [TestInitialize]
        public void Setup()
        {
            _kmlConverter = new GeoKmlConverter();
        }

        [TestMethod]
        public void ConvertToKml_With_Style_And_Definitions()
        {
            var style = new PictureMarkerSymbol();
            style.Name = "Stefania";
            style.IconUri = "http://www.basketbalnieuws.nl/kml/supermarket.png";
            style.Scale = 1.1;
            var testObject = new TestObject() { TestAsDescription = "Description", TestAsTitle = "Title", TestAsGeometry = GeoUtils.CreatePoint(52.574047699999994, 6.285734400000001), TestAsStyle = "Stefania" };
            GeoKmlConverter converter = new GeoKmlConverter();
            var xml = converter.ConvertToGeoKml<TestObject>(testObject, new List<ISymbol>() { style });
            xml.Should().Contain("#stefania");
        }


        [TestMethod]
        public void ConvertToKml_With_Style()
        {
            var testObject = new TestObject() { TestAsDescription = "Description", TestAsTitle = "Title", TestAsGeometry = GeoUtils.CreatePoint(10, 5), TestAsStyle = "Stefania" };
            GeoKmlConverter converter = new GeoKmlConverter();
            var xml = converter.ConvertToGeoKml<TestObject>(testObject);
            xml.Should().Contain("#stefania");
        }

        [TestMethod]
        public void ConvertToKml_With_Style_From_Enum()
        {
            var testObject = new TestObject2() { TestAsDescription = "Description", TestAsTitle = "Title", TestAsGeometry = GeoUtils.CreatePoint(10, 5), TestAsStyle = StyleName.Stefania };
            GeoKmlConverter converter = new GeoKmlConverter();
            var xml = converter.ConvertToGeoKml<TestObject2>(testObject);
            xml.Should().Contain("#stefania");
        }

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
            List<TestObject> objects = new List<TestObject>() { testObject, testObject2 };
            GeoKmlConverter converter = new GeoKmlConverter();
            var xml = converter.ConvertToGeoKml<List<TestObject>>(objects);
            StringAssert.StartsWith(xml, "<?xml");
        }

        [TestMethod]
        public void ReadLocationsFromDatabase()
        {
            var entities = new TestDatabaseEntities();
            var locations = entities.Locations.AsEnumerable();
            Assert.AreEqual(9, locations.Count());
        }

        [TestMethod]
        public void ConvertOneLocation()
        {
            var entities = new TestDatabaseEntities();
            var locations = entities.Locations.AsEnumerable();
            var location = locations.First();
            var xml = _kmlConverter.ConvertToGeoKml<Locations>(location);
            var expected = "<coordinates>53.2086992259963,6.56771600246429</coordinates>";
            StringAssert.Contains(xml, expected);
        }

    }
}