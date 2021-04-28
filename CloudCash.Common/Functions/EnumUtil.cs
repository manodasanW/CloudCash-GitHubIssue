using CloudCash.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CloudCash.Common.Functions
{
    public static class EnumUtil
    {
        public static Type GetDataClass<T>(this T e) where T : IConvertible
        {
            if (e is Enum)
            {
                Type type = e.GetType();

                foreach (int val in Enum.GetValues(type))
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));

                        if (memInfo[0].GetCustomAttributes(typeof(DataClassAttribute), false).FirstOrDefault() is DataClassAttribute dataClassAttribute)
                        {
                            return dataClassAttribute.DataClassType;
                        }
                    }
                }
            }

            return null;
        }

        public static string GetLocalizationStringValue<T>(this T e) where T : IConvertible
        {
            if (e is Enum)
            {
                Type type = e.GetType();

                foreach (int val in Enum.GetValues(type))
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));

                        if (memInfo[0].GetCustomAttributes(typeof(LocalizationStringAttribute), false).FirstOrDefault() is LocalizationStringAttribute stringValueAttribute)
                        {
                            return stringValueAttribute.StringValue;
                        }
                    }
                }
            }

            return null;
        }

        public static byte GetVatValue<T>(this T e) where T : IConvertible
        {
            if (e is Enum)
            {
                Type type = e.GetType();

                foreach (int val in Enum.GetValues(type))
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));

                        if (memInfo[0].GetCustomAttributes(typeof(VatValueAttribute), false).FirstOrDefault() is VatValueAttribute vatValueAttribute)
                        {
                            return vatValueAttribute.VatValue;
                        }
                    }
                }
            }

            return 0;
        }

        public static bool GetValidity<T>(this T e) where T : IConvertible
        {
            if (e is Enum)
            {
                Type type = e.GetType();

                foreach (int val in Enum.GetValues(type))
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));

                        if (memInfo[0].GetCustomAttributes(typeof(ValidityAttribute), false).FirstOrDefault() is ValidityAttribute validityAttribute)
                        {
                            return validityAttribute.IsValid;
                        }
                    }
                }
            }

            return false;
        }

        public static List<T> EnumToList<T>(this T e) where T : IConvertible
        {
            if (e is Enum)
            {
                Type type = e.GetType();

                return new(Enum.GetValues(type).Cast<T>());
            }

            return null;
        }
    }
}
