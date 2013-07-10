using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoKmlLibrary
{
    public class GeoKmlException : Exception
    {
        public GeoKmlException(string message) : base(message)
        {
        }
    }
}