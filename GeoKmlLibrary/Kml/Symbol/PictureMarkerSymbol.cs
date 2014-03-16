using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
namespace GeoKmlLibrary.Kml.Symbol
{
    public class PictureMarkerSymbol : Symbol
    {
        public double Scale { get; set; }
        public string IconUri { get; set; }

        #region IKml Members

        public override XElement ToKml()
        {
            var element = new XElement("Style");
            element.Add(new XAttribute("id", Name));
            var iconStyleElement = new XElement("IconStyle");
            iconStyleElement.Add(new XElement("scale", Scale));
            var iconElement = new XElement("Icon");
            iconElement.Add(new XElement("href", IconUri));
            iconStyleElement.Add(iconElement);
            element.Add(iconStyleElement);
            return element;
        }

        #endregion
    }
}
