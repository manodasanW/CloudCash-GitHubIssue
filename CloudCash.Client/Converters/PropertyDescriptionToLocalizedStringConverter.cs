using CloudCash.Common.Functions;
using Microsoft.UI.Xaml.Data;
using System;

namespace CloudCash.Client.Converters
{
    public sealed class PropertyDescriptionToLocalizedStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is null)
                return "";

            string propertyName = parameter as string;

            return Localization.GetLocalizedString(AttributeReader.GetPropertyDescriptionValue(value, propertyName));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}