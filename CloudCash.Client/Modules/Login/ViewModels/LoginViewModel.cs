using CloudCash.BL.DbAccess;
using CloudCash.BL.DTOs.Users;
using CloudCash.Common.Enums;
using CloudCash.Common.Exceptions;
using CloudCash.Common.Functions;
using CloudCash.Common.MVVM;
using CloudCash.Interface.BL;
using CloudCash.Interface.Common;

namespace CloudCash.Client.Modules.Login.ViewModels
{
    public class LoginViewModel : ViewModelBase, IUserControlViewModelBase
    {
        private readonly DbUser _dbUser;

        private UserDetailModel _insertedUser = new();
        public UserDetailModel InsertedUser
        {
            get => _insertedUser;
            set
            {
                _insertedUser = value;
                OnPropertyChanged();
            }
        }

        private bool _showPassword;
        public bool ShowPassword
        {
            get => _showPassword;
            set
            {
                _showPassword = value;
                OnPropertyChanged();
            }
        }

        public LoginViewModel(IMessenger messenger, DbUser dbUser, bool showPassword = true) : base(messenger)
        {
            _dbUser = dbUser;
            ShowPassword = showPassword;
        }

        public IListModelBase GetData() => InsertedUser;

        public void CheckData()
        {
            RunUnderBusyDialog(() =>
            {
                CheckNickInerted();
                CheckPassword();
            });
        }

        private void CheckNickInerted()
        {
            if (string.IsNullOrEmpty(InsertedUser.NickName))
                throw new ValidationException(
                    Localization.GetLocalizedString(InsertedUser.GetPropertyStringValue(nameof(InsertedUser.NickName))),
                    Localization.GetLocalizedString(LocalizationStrings.NotInserted));
        }

        private void CheckPassword()
        {
            var userFromDb = _dbUser.GetUserByNick(InsertedUser.NickName);

            CheckUserExist(userFromDb);

            if (ShowPassword && !Crypto.Decrypt(userFromDb.Salt, userFromDb.Hash, InsertedUser.Password))
                throw new ValidationException(
                    $"{Localization.GetLocalizedString(InsertedUser.GetPropertyStringValue(nameof(InsertedUser.NickName)))}, {Localization.GetLocalizedString(InsertedUser.GetPropertyStringValue(nameof(InsertedUser.Password)))}",
                    Localization.GetLocalizedString(LocalizationStrings.NickPasswordMismatch));

            InsertedUser = userFromDb;
        }

        private void CheckUserExist(UserDetailModel userFromDb)
        {
            if (userFromDb.Salt is null || userFromDb.Hash is null)
                throw new ValidationException(
                    Localization.GetLocalizedString(InsertedUser.GetPropertyStringValue(nameof(InsertedUser.NickName))),
                    Localization.GetLocalizedString(LocalizationStrings.UserNotExist));
        }

        public void DoBeforeCheck() { }
    }
}
