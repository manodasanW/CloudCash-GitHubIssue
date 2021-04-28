using CloudCash.Common.Enums;
using CloudCash.Common.Exceptions;
using CloudCash.Common.Functions;
using CloudCash.Common.ModelBase;
using System.Collections.ObjectModel;
using System.Linq;

namespace CloudCash.BL.DTOs.TableCategories
{
    public record TableCategoryModel : CategoryModelBase
    {
        protected void CheckValues() { base.CheckValues(); }

        public void CheckValues(ObservableCollection<TableCategoryModel> tableCategories)
        {
            CheckValues();

            if (tableCategories.FirstOrDefault(x => string.Equals(x.Name, Name, System.StringComparison.OrdinalIgnoreCase) && x.ID != ID) is not null)
                throw new ValidationException(
                    Localization.GetLocalizedString(GetPropertyStringValue(nameof(Name))),
                    Localization.GetLocalizedString(LocalizationStrings.DuplicatedValue));
        }
    }
}
