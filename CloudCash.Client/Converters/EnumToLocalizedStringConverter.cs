using CloudCash.Common.Enums;
using CloudCash.Common.Functions;
using Microsoft.UI.Xaml.Data;
using System;

namespace CloudCash.Client.Converters
{
    public class EnumToLocalizedStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is null || value is not VatLevel)
                return 0;

            return Localization.GetLocalizedString(((VatLevel)value).GetLocalizationStringValue());
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
