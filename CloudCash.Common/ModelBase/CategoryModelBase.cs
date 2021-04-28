using CloudCash.Common.Attributes;
using CloudCash.Common.Enums;
using CloudCash.Common.Functions;
using System.ComponentModel.DataAnnotations;

namespace CloudCash.Common.ModelBase
{
    public record CategoryModelBase : ListModelBase
    {
        [Required]
        [LocalizationString("CategoryModelBase/Name/Header")]
        public string Name { get; set; }

        public override void CheckValues()
        {
            if (string.IsNullOrEmpty(Name))
                throw new Exceptions.ValidationException(
                    Localization.GetLocalizedString(GetPropertyStringValue(nameof(Name))),
                    Localization.GetLocalizedString(LocalizationStrings.NullOrEmpty));
        }
    }
}
