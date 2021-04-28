using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace CloudCash.Client.Converters
{
    /// <summary>
    /// Switch visibility based on value, which is bool. Switch is enabled
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var param = parameter is string val ? val : string.Empty;

            if (value is bool boolValue)
                if (param.ToLower() is "not")
                    return boolValue ? Visibility.Collapsed : Visibility.Visible;
                else
                    return boolValue ? Visibility.Visible : Visibility.Collapsed;
            
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
