using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoKmlLibrary
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple= false, Inherited = true)]
    public class GeoKmlTitleAttribute: Attribute
    {
        public GeoKmlTitleAttribute(string propertyNameWithStringUsedForTitle = "")
        {
            PropertyName = propertyNameWithStringUsedForTitle;
        }

        public string PropertyName { get; private set; }
    }
}