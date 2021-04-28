using Microsoft.UI.Xaml.Data;
using System;

namespace CloudCash.Client.Converters
{
    public class BoolToMenuItemSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool boolValue)
                return boolValue ? 80 : 40;

            return 80;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
