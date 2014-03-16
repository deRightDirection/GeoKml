using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeoKmlLibrary.Kml.Symbol;
using System.Xml.Linq;
using FluentAssertions;
namespace GeoKmlLibrary.UnitTests.Kml.Symbol
{
    [TestClass]
    public class PictureMarkerSymbolTest
    {
        [TestMethod]
        public void ToXml_Is_Correct()
        {
            var symbol = new PictureMarkerSymbol();
            symbol.Name = "SuperMarket";
            symbol.Scale = 1.1;
            symbol.IconUri = "http://www.basketbalnieuws.nl/kml/supermarket.png";
            XElement xml = symbol.ToKml();
            xml.Should().HaveAttribute("id", "supermarket")
            .And.HaveElement("IconStyle");
        }
    }
}