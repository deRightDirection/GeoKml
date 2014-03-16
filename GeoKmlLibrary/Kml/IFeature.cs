
using GeoKmlLibrary.Kml.Geometry;
using GeoKmlLibrary.Kml.Symbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoKmlLibrary.Kml
{
    public interface IFeature : IKml
    {
        IGeometry Geometry { get; set; }

        string Name { get; set; }

        string Description { get; set; }

        string SymbolName { get; set; }
    }
}