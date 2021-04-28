using Microsoft.UI.Xaml.Data;
using System;

namespace CloudCash.Client.Converters
{
    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) => value is bool boolVal && !boolVal;

        public object ConvertBack(object value, Type targetType, object parameter, string language) => Convert(value, targetType, parameter, language);
    }
}
