using CloudCash.Common.Enums;
using Windows.ApplicationModel.Resources.Core;

namespace CloudCash.Common.Functions
{
    public class Localization
    {
        public static string GetLocalizedString(LocalizationStrings localizationStringReference)
        {
            return GetLocalizedString(localizationStringReference.GetLocalizationStringValue());
        }

        public static string GetLocalizedStringForMenuItem(string reference)
        {
            return GetLocalizedString($"{LocalizationStrings.MenuItem.GetLocalizationStringValue()}{reference}");
        }

        public static string GetLocalizedString(string reference)
        {
            var resources = ResourceManager.Current.MainResourceMap.GetSubtree("Resources");
            return resources.GetValue(reference, new ResourceContext())?.ValueAsString ?? string.Empty;
        }
    }
}
