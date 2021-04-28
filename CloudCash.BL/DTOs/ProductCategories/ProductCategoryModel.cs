using CloudCash.Common.Attributes;
using CloudCash.Common.Enums;
using CloudCash.Common.Exceptions;
using CloudCash.Common.Functions;
using CloudCash.Common.ModelBase;
using System.Collections.ObjectModel;
using System.Linq;

namespace CloudCash.BL.DTOs.ProductCategories
{
    public record ProductCategoryModel : CategoryModelBase
    {
        [LocalizationString("ProductCategoryModel/PrintSeparately/Header")]
        public bool PrintSeparately { get; set; }

        protected void CheckValues() { base.CheckValues(); }

        public void CheckValues(ObservableCollection<ProductCategoryModel> productCategories)
        {
            CheckValues();

            if (productCategories.FirstOrDefault(x => string.Equals(x.Name, Name, System.StringComparison.OrdinalIgnoreCase) && x.ID != ID) is not null)
                throw new ValidationException(
                    Localization.GetLocalizedString(GetPropertyStringValue(nameof(Name))),
                    Localization.GetLocalizedString(LocalizationStrings.DuplicatedValue));
        }
    }
}
