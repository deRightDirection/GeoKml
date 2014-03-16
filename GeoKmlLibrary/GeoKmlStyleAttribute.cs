using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoKmlLibrary
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple= false, Inherited = true)]
    public class GeoKmlSymbolAttribute: Attribute
    {
        public GeoKmlSymbolAttribute(string propertyNameWithStringUsedForStyle = "")
        {
            PropertyName = propertyNameWithStringUsedForStyle;
        }

        public string PropertyName { get; private set; }
    }
}