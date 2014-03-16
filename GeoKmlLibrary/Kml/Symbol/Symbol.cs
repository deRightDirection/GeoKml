using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GeoKmlLibrary.Kml.Symbol
{
    public abstract class Symbol : ISymbol
    {
        private string _name;
        #region ISymbol Members

        public string Name 
        {
            get
            {
                return _name.ToLowerInvariant();
            }
            set
            {
                _name = value;
            }
        }

        #endregion

        #region IKml Members

        public abstract XElement ToKml();

        #endregion
    }
}