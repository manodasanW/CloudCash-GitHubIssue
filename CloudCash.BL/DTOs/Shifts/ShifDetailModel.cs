using CloudCash.BL.DTOs.Users;
using CloudCash.Common.Attributes;
using CloudCash.Common.Enums;
using CloudCash.Common.Functions;

namespace CloudCash.BL.DTOs.Shifts
{
    public record ShiftDetailModel : ShiftListModel
    {
        [LocalizationString("ShiftDetailModel/User/Header")]
        public UserListModel User { get; set; } = new();

        [LocalizationString("ShiftDetailModel/CashValue/Header")]
        public uint CashValue { get; set; }

        public override void CheckValues()
        {
            if (User is null || User.ID == 0)
                throw new Common.Exceptions.ValidationException(
                    Localization.GetLocalizedString(GetPropertyStringValue(nameof(User))),
                    Localization.GetLocalizedString(LocalizationStrings.NotInserted));
        }
    }
}
