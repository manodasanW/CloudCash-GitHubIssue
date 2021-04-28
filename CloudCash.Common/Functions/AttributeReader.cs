using CloudCash.Common.Attributes;
using System;
using System.ComponentModel;
using System.Reflection;

namespace CloudCash.Common.Functions
{
    public static class AttributeReader
    {
        public static string GetPropertyLocalizationStringValue(object objectWithProperty, string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return new ArgumentNullException(propertyName).ToString();

            Type type = objectWithProperty.GetType();

            PropertyInfo property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (property is null)
                return new ArgumentOutOfRangeException("Property", propertyName,
                    "Property \"" + propertyName + "\" not found in type \"" + type.Name + "\".").ToString();

            if (!property.IsDefined(typeof(LocalizationStringAttribute), true))
                return new ArgumentOutOfRangeException("Property", propertyName,
                    "Property \"" + propertyName + "\" of type \"" + type.Name + "\"" +
                    " has no associated Description attribute.").ToString();

            return ((LocalizationStringAttribute)property.GetCustomAttributes(typeof(LocalizationStringAttribute), true)[0]).StringValue;
        }

        public static string GetPropertyDescriptionValue(object objectWithProperty, string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return new ArgumentNullException(propertyName).ToString();

            Type type = objectWithProperty.GetType();

            PropertyInfo property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (property is null)
                return new ArgumentOutOfRangeException("Property", propertyName,
                    "Property \"" + propertyName + "\" not found in type \"" + type.Name + "\".").ToString();

            if (!property.IsDefined(typeof(DescriptionAttribute), true))
                return new ArgumentOutOfRangeException("Property", propertyName,
                    "Property \"" + propertyName + "\" of type \"" + type.Name + "\"" +
                    " has no associated Description attribute.").ToString();

            return ((DescriptionAttribute)property.GetCustomAttributes(typeof(DescriptionAttribute), true)[0]).Description;
        }

        public static byte GetPropertyVatValueValue(object objectWithProperty, string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                //return new ArgumentNullException(propertyName).ToString();
                return 0;

            Type type = objectWithProperty.GetType();

            PropertyInfo property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (property is null)
                return 0;
                //return new ArgumentOutOfRangeException("Property", propertyName,
                //    "Property \"" + propertyName + "\" not found in type \"" + type.Name + "\".").ToString();

                if (!property.IsDefined(typeof(VatValueAttribute), true))
                    return  0;
                //return new ArgumentOutOfRangeException("Property", propertyName,
                //    "Property \"" + propertyName + "\" of type \"" + type.Name + "\"" +
                //    " has no associated Description attribute.").ToString();

            return ((VatValueAttribute)property.GetCustomAttributes(typeof(VatValueAttribute), true)[0]).VatValue;
        }

        public static bool GetPropertyValidityValue(object objectWithProperty, string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                //return new ArgumentNullException(propertyName).ToString();
                return false;

            Type type = objectWithProperty.GetType();

            PropertyInfo property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (property is null)
                //return new ArgumentOutOfRangeException("Property", propertyName,
                //    "Property \"" + propertyName + "\" not found in type \"" + type.Name + "\".").ToString();
                return false;

            if (!property.IsDefined(typeof(ValidityAttribute), true))
                return false;
                //return new ArgumentOutOfRangeException("Property", propertyName,
                //    "Property \"" + propertyName + "\" of type \"" + type.Name + "\"" +
                //    " has no associated Description attribute.").ToString();

            return ((ValidityAttribute)property.GetCustomAttributes(typeof(ValidityAttribute), true)[0]).IsValid;
        }
    }
}
