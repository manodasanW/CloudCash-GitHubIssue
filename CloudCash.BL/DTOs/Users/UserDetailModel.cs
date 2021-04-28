using CloudCash.Common.Attributes;
using CloudCash.Common.Enums;
using CloudCash.Common.Functions;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace CloudCash.BL.DTOs.Users
{
    public record UserDetailModel : UserListModel
    {
        [LocalizationString("UserDetailModel/Rights/Header")]
        public Right Rights { get; set; } = Right.None;

        public byte[] Hash { get; set; }

        public byte[] Salt { get; set; }

        [LocalizationString("UserDetailModel/NewPassword/Header")]
        [Description("UserDetailModel/NewPassword/Tooltip")]
        public string NewPassword { get; set; } = string.Empty;

        [LocalizationString("UserDetailModel/NewPasswordAgain/Header")]
        public string NewPasswordAgain { get; set; }

        [LocalizationString("UserDetailModel/Password/Header")]
        public string Password { get; set; } = string.Empty;

        public void CheckValues(ObservableCollection<UserDetailModel> users, bool checkPassword)
        {
            base.CheckValues(new(users.Cast<UserListModel>().ToList()));

            if (checkPassword)
            {
                if (NewPassword.Length < 6) // Insp get from constants???
                    throw new Common.Exceptions.ValidationException(
                        Localization.GetLocalizedString(GetPropertyStringValue(nameof(NewPassword))),
                        Localization.GetLocalizedString(LocalizationStrings.TooShort));

                if (NewPassword != NewPasswordAgain)
                    throw new Common.Exceptions.ValidationException(
                        $"{Localization.GetLocalizedString(GetPropertyStringValue(nameof(NewPassword)))}, {Localization.GetLocalizedString(GetPropertyStringValue(nameof(NewPasswordAgain)))}",
                        Localization.GetLocalizedString(LocalizationStrings.AreNotSame));
            }

            if (Rights is Right.None)
                throw new Common.Exceptions.ValidationException(
                    Localization.GetLocalizedString(GetPropertyStringValue(nameof(Rights))),
                    Localization.GetLocalizedString(LocalizationStrings.NotInserted));

        }

        public override void DoBeforeCheck()
        {
            base.DoBeforeCheck();

            if (!string.IsNullOrEmpty(NewPassword))
            {
                var (salt, hash) = Crypto.Encrypt(NewPassword);
                Hash = hash;
                Salt = salt;
            }
        }

        public static UserDetailModel ConvertToUserDetailModel(UserListModel userListModel)
        {
            return new UserDetailModel()
            {
                ID = userListModel.ID,
                FirstName = userListModel.FirstName,
                LastName = userListModel.LastName
            };
        }

        public static UserDetailModel FirstUseUserDetailModel()
        {
            return new()
            {
                FirstName = "default",
                LastName = "default",
                NickName = "defaut",
                NewPassword = "default",
                NewPasswordAgain = "default",
                Rights = Right.ShowSettings
            };
        }
    }
}
