using CloudCash.Common.Attributes;
using CloudCash.Common.Enums;
using CloudCash.Common.Functions;
using CloudCash.Common.ModelBase;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace CloudCash.BL.DTOs.Users
{
    public record UserListModel : ListModelBase
    {
        [LocalizationString("UserListModel/FirstName/Header")]
        public string FirstName { get; set; }

        [LocalizationString("UserListModel/LastName/Header")]
        public string LastName { get; set; }

        [LocalizationString("UserListModel/NickName/Header")]
        public string NickName { get; set; }

        [Obsolete("Don't use", true)]
        public override void CheckValues()
        {
            throw new NotImplementedException();
        }

        public virtual void CheckValues(ObservableCollection<UserListModel> users)
        {
            if (string.IsNullOrEmpty(FirstName))
                throw new Common.Exceptions.ValidationException(
                    Localization.GetLocalizedString(GetPropertyStringValue(nameof(FirstName))),
                    Localization.GetLocalizedString(LocalizationStrings.NullOrEmpty));

            if (string.IsNullOrEmpty(LastName))
                throw new Common.Exceptions.ValidationException(
                    Localization.GetLocalizedString(GetPropertyStringValue(nameof(LastName))),
                    Localization.GetLocalizedString(LocalizationStrings.NullOrEmpty));

            if (string.IsNullOrEmpty(NickName))
                throw new Common.Exceptions.ValidationException(
                    Localization.GetLocalizedString(GetPropertyStringValue(nameof(NickName))),
                    Localization.GetLocalizedString(LocalizationStrings.NullOrEmpty));

            if (users.FirstOrDefault(x => x.NickName == NickName && x.ID != ID) is not null)
                throw new Common.Exceptions.ValidationException(
                    Localization.GetLocalizedString(GetPropertyStringValue(nameof(NickName))),
                    Localization.GetLocalizedString(LocalizationStrings.DuplicatedValue));
        }
    }
}
