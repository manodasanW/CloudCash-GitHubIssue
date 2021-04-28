using CloudCash.BL.DTOs.TableCategories;
using CloudCash.Common.Attributes;
using CloudCash.Common.Enums;
using CloudCash.Common.Functions;
using CloudCash.Common.ModelBase;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace CloudCash.BL.DTOs.TableInfo
{
    public record TableInfoDetailModel : ListModelBase
    {
        [LocalizationString("TableInfoDetailModel/Name/Header")]
        public string Name { get; set; }

        [LocalizationString("TableInfoDetailModel/Size/Header")]
        public byte Size { get; set; }

        [LocalizationString("TableInfoDetailModel/PositionX/Header")]
        public short PositionX { get; set; }

        [LocalizationString("TableInfoDetailModel/PositionY/Header")]
        public short PositionY { get; set; }

        [LocalizationString("TableInfoDetailModel/Category/Header")]
        public TableCategoryModel Category { get; set; } = new();

        public TableInfoDetailModel()
        {

        }

        public TableInfoDetailModel(TableCategoryModel defaultValue)
        {
            Category = defaultValue;
        }

        public void CheckValues(ObservableCollection<TableInfoDetailModel> tableInfos)
        {
            if (string.IsNullOrEmpty(Name))
                throw new Common.Exceptions.ValidationException(
                    Localization.GetLocalizedString(GetPropertyStringValue(nameof(Name))),
                    Localization.GetLocalizedString(LocalizationStrings.NullOrEmpty));

            if (Size is 0)
                throw new Common.Exceptions.ValidationException(
                    Localization.GetLocalizedString(GetPropertyStringValue(nameof(Size))),
                    Localization.GetLocalizedString(LocalizationStrings.NullOrEmpty));

            if (PositionX < 0)
                throw new Common.Exceptions.ValidationException(
                    Localization.GetLocalizedString(GetPropertyStringValue(nameof(PositionX))),
                    Localization.GetLocalizedString(LocalizationStrings.BelowZero));

            if (PositionY < 0)
                throw new Common.Exceptions.ValidationException(
                    Localization.GetLocalizedString(GetPropertyStringValue(nameof(PositionY))),
                    Localization.GetLocalizedString(LocalizationStrings.BelowZero));

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
