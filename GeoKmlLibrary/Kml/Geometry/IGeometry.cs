using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoKmlLibrary.Kml.Geometry
{
    public interface IGeometry : IKml
    {
        GeometryType GeometryType { get; }
        IEnumerable<Point> Points { get;}
    }
}