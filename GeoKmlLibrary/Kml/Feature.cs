using GeoKmlLibrary.Kml.Geometry;
using GeoKmlLibrary.Kml.Symbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GeoKmlLibrary.Kml
{
    public class Feature : IFeature
    {
        public IGeometry Geometry { get; set; }

        #region IKml Members

        public XElement ToKml()
        {
            var placeMark = new XElement("Placemark");
            placeMark.Add(new XElement("name", Name));
            placeMark.Add(new XElement("description", Description));
            if(!string.IsNullOrEmpty(SymbolName))
            {
                var url = string.Format("#{0}", SymbolName.ToLowerInvariant());
                placeMark.Add(new XElement("styleUrl", url));
            }
            placeMark.Add(Geometry.ToKml());
            return placeMark;
        }

        #endregion

        #region IFeature Members
        public string Name {get;set;}

        public string Description {get;set;}

        public string SymbolName {get;set;}

        #endregion
    }
}
