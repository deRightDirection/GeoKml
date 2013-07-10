using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpKml.Base;
using SharpKml.Dom;
using System.Data.Spatial;
using System.Reflection;

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
        public string ConvertToGeoKml<T>(T objectToConvert)
        {
            Kml kml = new Kml();
            var typeToConvert = objectToConvert.GetType();
            var typeIsFromList = GetTypeOfList(typeToConvert);
            if (typeIsFromList != null)
            {
                Folder folder = CreateKmlFolder<T>(objectToConvert);
                kml.Feature = folder;
            }
            else
            {
                Placemark placeMark = CreatePlaceMark(objectToConvert);
                kml.Feature = placeMark;
            }
            Serializer serializer = new Serializer();
            serializer.Serialize(kml);
            return serializer.Xml;
        }

        private Folder CreateKmlFolder<T>(T objectToConvert)
        {
            var folder = new Folder();
            var list = objectToConvert as IEnumerable;
            foreach (var item in list)
            {
                var placeMark = CreatePlaceMark(item);
                folder.AddFeature(placeMark);
            }
            return folder;
        }

        private Placemark CreatePlaceMark(object objectToConvert)
        {
            Placemark place = new Placemark();
            place.Description = GetDescription(objectToConvert);
            place.Geometry = GetGeometry(objectToConvert);
            place.Name = GetTitle(objectToConvert); 
            return place;
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

        private Description GetDescription(object objectToConvert)
        {
            var description = new Description();
            Type type = objectToConvert.GetType();
            string geometryClassAttributePropertyName = GetAttributePropertyName<GeoKmlDescriptionAttribute>(type);
            foreach (var property in type.GetProperties())
            {
                var geoKmlDescriptionAttribute = property.GetCustomAttribute<GeoKmlDescriptionAttribute>();
                if (geoKmlDescriptionAttribute != null)
                {
                    if (property.PropertyType == typeof(string))
                    {
                        description.Text = property.GetValue(objectToConvert) as string;
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
                            description.Text = property.GetValue(objectToConvert) as string;
                        }
                    }
                }
            }
            if (string.IsNullOrEmpty(description.Text))
            {
                throw new GeoKmlException("Object doesn't have string property with GeoKmlDescription attribute");
            }
            return description;
        }

        private Point GetGeometry(object objectToConvert)
        {
            Type type = objectToConvert.GetType();
            Point point = null;
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
                Vector vector = new Vector();
                vector.Latitude = location.Latitude.Value;
                vector.Longitude = location.Longitude.Value;
                point = new Point();
                point.Coordinate = vector;
            }
            if (point == null)
            {
                throw new GeoKmlException("Object doesn't have geometry property with GeoKmlGeometry attribute");
            }
            return point;
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
