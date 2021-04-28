using Microsoft.UI.Xaml.Data;
using System;

namespace CloudCash.Client.Converters
{
    public class NumberToNumberConverter<From, To> : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value is From ? System.Convert.ChangeType(value, typeof(To)) : 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            try
            {
                return value is To ? System.Convert.ChangeType(value, typeof(From)) : 0;
            }
            catch
            { return 0; }
        }
    }
}
