using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data.Entity.Spatial;
using GeoKmlLibrary.Kml.Symbol;
using GeoKmlLibrary.Kml;
using GeoKmlLibrary.Kml.Geometry;

namespace GeoKmlLibrary
{
    public class GeoKmlConverter
    {
        public T ConvertFromGeoKml<T>(string geokml)
        {
            throw new NotImplementedException("not implemented in version 1.0 of this library");
            return default(T);
        }

        /// <summary>
        /// Converts an object or a list of objects to geokml representation
        /// Only points are supported
        /// </summary>
        /// <typeparam name="T">objecttype</typeparam>
        /// <param name="objectToConvert">an object or list of objects</param>
        /// <returns>
        /// kml as xml-structure
        /// </returns>
        public string ConvertToGeoKml<T>(T objectToConvert, IEnumerable<ISymbol> styles = null)
        {
            var mapLayer = new MapLayer();;
            var typeToConvert = objectToConvert.GetType();
            var typeIsFromList = GetTypeOfList(typeToConvert);
            if (typeIsFromList != null)
            {
                AddFeatures<T>(mapLayer, objectToConvert);
            }   
            else
            {
                AddFeature(mapLayer, objectToConvert);
            }
            if (styles != null)
            {
                styles.ToList().ForEach(s => mapLayer.Symbols.Add(s));
            }
            return mapLayer.ToKml();
        }

        private void AddFeatures<T>(MapLayer mapLayer, T objectToConvert)
        {
            var list = objectToConvert as IEnumerable;
            foreach (var item in list)
            {
                AddFeature(mapLayer, item);
            }
        }

        private void AddFeature(MapLayer mapLayer, object objectToConvert)
        {
            var feature = new Feature();
            feature.Description = GetDescription(objectToConvert);
            feature.Geometry = GetGeometry(objectToConvert);
            feature.Name = GetTitle(objectToConvert);
            var styleUrl = GetStyleUrl(objectToConvert);
            if (!string.IsNullOrEmpty(styleUrl))
            {
                feature.SymbolName = styleUrl;
            }
            mapLayer.Features.Add(feature);
        }

        private string GetStyleUrl(object objectToConvert)
        {
            Type type = objectToConvert.GetType();
            string styleClassAttributePropertyName = GetAttributePropertyName<GeoKmlSymbolAttribute>(type);
            foreach (var property in type.GetProperties())
            {
                var geoKmlStyleAttribute = property.GetCustomAttribute<GeoKmlSymbolAttribute>();
                if (geoKmlStyleAttribute != null)
                {
                    if (property.PropertyType == typeof(string))
                    {
                        var result = property.GetValue(objectToConvert) as string;
                        if (string.IsNullOrEmpty(result))
                        {
                            return null;
                        }
                        return result.ToLowerInvariant();
                    }
                    if(property.PropertyType.IsEnum)
                    {
                        var result = property.GetValue(objectToConvert).ToString();
                        if (string.IsNullOrEmpty(result))
                        {
                            return null;
                        }
                        return result.ToLowerInvariant();
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(styleClassAttributePropertyName))
                    {
                        if (string.Equals(styleClassAttributePropertyName, property.Name, StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (property.PropertyType == typeof(string))
                            {
                                var result = property.GetValue(objectToConvert) as string;
                                if(string.IsNullOrEmpty(result))
                                {
                                    return null;
                                }
                                return result.ToLowerInvariant();
                            }
                            if (property.PropertyType.IsEnum)
                            {
                                var result = property.GetValue(objectToConvert).ToString();
                                if(string.IsNullOrEmpty(result))
                                {
                                    return null;
                                }
                                return result.ToLowerInvariant();
                            }
                        }
                    }
                }
            }
            return null;
        }

        private string GetTitle(object objectToConvert)
        {
            string title = null;
            Type type = objectToConvert.GetType();
            string geometryClassAttributePropertyName = GetAttributePropertyName<GeoKmlTitleAttribute>(type);
            foreach (var property in type.GetProperties())
            {
                var geoKmlTitleAttribute = property.GetCustomAttribute<GeoKmlTitleAttribute>();
                if (geoKmlTitleAttribute != null)
                {
                    if (property.PropertyType == typeof(string))
                    {
                        return property.GetValue(objectToConvert) as string;
                    }
                    else
                    {
                        throw new GeoKmlException("Property with title is not type of string");
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(geometryClassAttributePropertyName))
                    {
                        if (string.Equals(geometryClassAttributePropertyName, property.Name, StringComparison.InvariantCultureIgnoreCase))
                        {
                            return property.GetValue(objectToConvert) as string;
                        }
                    }
                }
            }
            if (string.IsNullOrEmpty(title))
            {
                throw new GeoKmlException("Object doesn't have string property with GeoKmlTitle attribute");
            }
            return title;
        }

        private string GetDescription(object objectToConvert)
        {
            Type type = objectToConvert.GetType();
            string geometryClassAttributePropertyName = GetAttributePropertyName<GeoKmlDescriptionAttribute>(type);
            foreach (var property in type.GetProperties())
            {
                var geoKmlDescriptionAttribute = property.GetCustomAttribute<GeoKmlDescriptionAttribute>();
                if (geoKmlDescriptionAttribute != null)
                {
                    if (property.PropertyType == typeof(string))
                    {
                        return property.GetValue(objectToConvert) as string;
                    }
                    else
                    {
                        throw new GeoKmlException("Property with description is not type of string");
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(geometryClassAttributePropertyName))
                    {
                        if (string.Equals(geometryClassAttributePropertyName, property.Name, StringComparison.InvariantCultureIgnoreCase))
                        {
                            return property.GetValue(objectToConvert) as string;
                        }
                    }
                }
            }
            throw new GeoKmlException("Object doesn't have string property with GeoKmlDescription attribute");
        }

        private IGeometry GetGeometry(object objectToConvert)
        {
            Type type = objectToConvert.GetType();
            DbGeography location = null;
            string geometryClassAttributePropertyName = GetAttributePropertyName<GeoKmlGeometryAttribute>(type);
            foreach (var property in type.GetProperties())
            {
                var geoJsonAttribute = property.GetCustomAttribute<GeoKmlGeometryAttribute>();
                if (geoJsonAttribute != null)
                {
                    if (property.PropertyType == typeof(DbGeography))
                    {
                        location = property.GetValue(objectToConvert) as DbGeography;
                    }
                    else
                    {
                        throw new GeoKmlException("Property with geometry is not type of SqlGeography");
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(geometryClassAttributePropertyName))
                    {
                        if (string.Equals(geometryClassAttributePropertyName, property.Name, StringComparison.InvariantCultureIgnoreCase))
                        {
                            location = property.GetValue(objectToConvert) as DbGeography;
                        }
                    }
                }
            }
            if (location != null)
            {
                var typename = location.SpatialTypeName;
                if (typename.Equals("Point"))
                {
                    var point  = new Point();
                    point.Latitude = location.Latitude.Value;
                    point.Longitude = location.Longitude.Value;
                    return point;
                }
            }
            throw new GeoKmlException("Object doesn't have geometry property with GeoKmlGeometry attribute");
        }

        private string GetAttributePropertyName<T>(Type type) where T:Attribute
        {
            var geoKmlAttribute = type.GetCustomAttribute<T>();
            if (geoKmlAttribute != null)
            {
                Type typeOfAttribute = geoKmlAttribute.GetType();
                PropertyInfo propertyNameInfo = typeOfAttribute.GetProperty("PropertyName");
                return propertyNameInfo.GetValue(geoKmlAttribute).ToString();
            }
            return null;
        }

        private Type GetTypeOfList(Type objectToConvert)
        {
            foreach (Type interfaceType in objectToConvert.GetInterfaces())
            {
                var isGenericType = interfaceType.IsGenericType;
                var isIlistOrIEnumerable = interfaceType.GetGenericTypeDefinition() == typeof(IList<>) || interfaceType.GetGenericTypeDefinition() == typeof(IEnumerable<>);
                if (isGenericType && isIlistOrIEnumerable)
                {
                    return objectToConvert.GetGenericArguments()[0];
                }
            }
            return null;
        }
    }
}
