using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeoKmlLibrary.Kml;
using GeoKmlLibrary.Kml.Symbol;
using GeoKmlLibrary.Kml.Geometry;

namespace GeoKmlLibrary.UnitTests.Kml
{
    [TestClass]
    public class MapLayerTest
    {
        [TestMethod]
        public void ToKml_Is_Correct()
        {
            var symbol = new PictureMarkerSymbol();
            symbol.Name = "SuperMarket";
            symbol.Scale = 1.1;
            symbol.IconUri = "http://www.basketbalnieuws.nl/kml/supermarket.png";
            var feature = new Feature();
            feature.Description = "Mannus";
            feature.Geometry = new Point(10.1, 10.3);
            feature.Name = "Stefania";
            feature.SymbolName = "Test";
            var mapLayer = new MapLayer();
            mapLayer.Symbols.Add(symbol);
            mapLayer.Features.Add(feature);
            var result = mapLayer.ToKml();
        }
    }
}