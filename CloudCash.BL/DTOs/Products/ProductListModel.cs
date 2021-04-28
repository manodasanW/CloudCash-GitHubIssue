using CloudCash.BL.DTOs.ProductCategories;
using CloudCash.Common.Attributes;
using CloudCash.Common.Enums;
using CloudCash.Common.Functions;
using CloudCash.Common.ModelBase;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace CloudCash.BL.DTOs.Products
{
    public record ProductListModel : ListModelBase
    {
        [LocalizationString("ProductListModel/Name/Header")]
        public string Name { get; set; }

        [LocalizationString("ProductListModel/Price/Header")]
        public int Price { get; set; }

        [LocalizationString("ProductListModel/Category/Header")]
        public ProductCategoryModel Category { get; set; } = new();

        [LocalizationString("ProductListModel/VatLevel/Header")]
        public VatLevel VatLevel { get; set; }

        public void CheckValues(ObservableCollection<ProductListModel> tableInfos)
        {
            if (VatLevel is VatLevel.None)
                throw new Common.Exceptions.ValidationException(
                    Localization.GetLocalizedString(GetPropertyStringValue(nameof(VatLevel))),
                    Localization.GetLocalizedString(LocalizationStrings.NullOrEmpty));

            if (string.IsNullOrEmpty(Name))
                throw new Common.Exceptions.ValidationException(
                    Localization.GetLocalizedString(GetPropertyStringValue(nameof(Name))),
                    Localization.GetLocalizedString(LocalizationStrings.NullOrEmpty));

            if (Category is null || string.IsNullOrEmpty(Category.Name))
                throw new Common.Exceptions.ValidationException(
                    Localization.GetLocalizedString(GetPropertyStringValue(nameof(Category))),
                    Localization.GetLocalizedString(LocalizationStrings.NotInserted));

            if (tableInfos.Where(x => string.Equals(x.Name, Name, StringComparison.OrdinalIgnoreCase) && x.ID != ID).FirstOrDefault(x => x.Category.Name == Category.Name) is not null)
                throw new Common.Exceptions.ValidationException(
                    $"{Localization.GetLocalizedString(GetPropertyStringValue(nameof(Name)))}, {Localization.GetLocalizedString(GetPropertyStringValue(nameof(Category)))}",
                    Localization.GetLocalizedString(LocalizationStrings.DuplicatedValue));
        }

        [Obsolete("Don't use", true)]
        public override void CheckValues()
        {
            throw new NotImplementedException();
        }
    }
}
