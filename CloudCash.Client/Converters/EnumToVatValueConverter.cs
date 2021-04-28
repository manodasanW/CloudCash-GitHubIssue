using CloudCash.Common.Enums;
using CloudCash.Common.Functions;
using Microsoft.UI.Xaml.Data;
using System;

namespace CloudCash.Client.Converters
{
    public class EnumToVatValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is null || value is not VatLevel)
                return 0;

            return ((VatLevel)value).GetVatValue();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
