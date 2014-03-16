using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoKmlLibrary.Kml.Symbol
{
    public interface ISymbol : IKml
    {
        string Name { get; set; }
    }
}
