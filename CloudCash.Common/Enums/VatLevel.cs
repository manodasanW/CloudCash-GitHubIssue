using CloudCash.Common.Attributes;

namespace CloudCash.Common.Enums
{
    public enum VatLevel
    {
        [LocalizationString("VatLevel/None")]
        [Validity(false)]
        [VatValue(0)]
        None,

        [LocalizationString("VatLevel/Base_2021")]
        [Validity(true)]
        [VatValue(21)]
        Base_2021,

        [LocalizationString("VatLevel/FirstLower_2021")]
        [Validity(true)]
        [VatValue(15)]
        FirstLower_2021,

        [LocalizationString("VatLevel/SecondLower_2021")]
        [Validity(true)]
        [VatValue(10)]
        SecondLower_2021
    }
}
