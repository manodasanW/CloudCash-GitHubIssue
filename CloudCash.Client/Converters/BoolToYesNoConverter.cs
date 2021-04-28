using CloudCash.Common.Enums;
using CloudCash.Common.Functions;
using Microsoft.UI.Xaml.Data;
using System;

namespace CloudCash.Client.Converters
{
    public class BoolToYesNoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var param = parameter is string val ? val : string.Empty;

            if (value is bool boolValue)
                return boolValue ? Localization.GetLocalizedString(LocalizationStrings.YesButton) : Localization.GetLocalizedString(LocalizationStrings.NoButton);

            return Localization.GetLocalizedString(LocalizationStrings.NoButton);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
