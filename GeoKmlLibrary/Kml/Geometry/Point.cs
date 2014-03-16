using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GeoKmlLibrary.Kml.Geometry
{
    public class Point : Geometry
    {
        public Point()
        {
            GeometryType = GeometryType.Point;
        }

        public Point(double longitude, double latitude) : base()
        {
            Longitude = longitude;
            Latitude = latitude;
        }

        #region IKml Members

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public override XElement ToKml()
        {
            var element = new XElement("Point");
            var coordinatesElement = new XElement("coordinates");
            var coordinate = string.Format("{0},{1}", Longitude.ToString(CultureInfo.InvariantCulture), Latitude.ToString(CultureInfo.InvariantCulture));
            coordinatesElement.Value = coordinate;
            element.Add(coordinatesElement);
            return element;
        }

        #endregion
    }
}
