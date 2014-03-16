using GeoKmlLibrary.Kml.Symbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GeoKmlLibrary.Kml
{
    public class MapLayer
    {
        public MapLayer()
        {
            Features = new List<IFeature>();
            Symbols = new List<ISymbol>();
        }
        public List<IFeature> Features { get; private set; }
        public List<ISymbol> Symbols { get; private set; }

        public string GetKml()
        {
            return ToKml().ToString();
        }

        #region IKml Members

        public string ToKml()
        {
            var declaration = new XDeclaration("1.0", "utf-8","yes");
            var document = new XDocument(declaration);
            var kml = new XElement("kml");
            kml.Add(new XAttribute("prefix","http://www.opengis.net/kml/2.2"));
            var element = new XElement("Document");
            foreach(var style in Symbols)
            {
                element.Add(style.ToKml());
            }
            foreach (var feature in Features)
            {
                element.Add(feature.ToKml());
            }
            kml.Add(element);
            document.Add(kml);
            var result = string.Concat(document.Declaration.ToString(), document.ToString());
            result = result.Replace("prefix", "xmlns");
            result = result.Replace(" standalone=\"yes\"", string.Empty);
            return result;
        }

        #endregion
    }
}
