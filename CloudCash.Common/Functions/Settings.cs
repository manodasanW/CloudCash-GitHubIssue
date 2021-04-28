using System.Runtime.CompilerServices;
using Windows.Storage;

namespace CloudCash.Common.Functions
{
    public static class Settings
    {
        public static void SaveSettingValue<T>(T value, [CallerMemberName] string valueName = null)
        {
            valueName.CheckNull();

            ApplicationData.Current.LocalSettings.Values[valueName] = value;
        }

        public static T ReadSettingsValue<T>([CallerMemberName] string valueName = null)
        {
            valueName.CheckNull();

            try
            {
                var res = ApplicationData.Current.LocalSettings.Values[valueName];

                return res is null ? default : (T)res;
            }
            catch
            {
                return default;
            }
        }
    }
}
