using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GeoKmlLibrary.Kml.Geometry
{
    public abstract class Geometry : IGeometry
    {
        #region IGeometry Members

        public IEnumerable<Point> Points
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IGeometry Members

        public GeometryType GeometryType {get; protected set;}

        #endregion

        #region IKml Members

        public abstract XElement ToKml();

        #endregion
    }
}
