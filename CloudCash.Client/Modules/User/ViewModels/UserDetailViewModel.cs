using CloudCash.BL.DbAccess;
using CloudCash.BL.DTOs.Users;
using CloudCash.Client.Modules.Settings.Messages;
using CloudCash.Client.Modules.User.Messages;
using CloudCash.Common.Attributes;
using CloudCash.Common.Enums;
using CloudCash.Common.Functions;
using CloudCash.Common.MVVM;
using CloudCash.Interface.Common;
using System.Collections.Generic;
using System.Windows.Input;

namespace CloudCash.Client.Modules.User.ViewModels
{
    public class UserDetailViewModel : ViewModelBase
    {
        private readonly DbUser _dbUser;

        #region Props

        private UserDetailModel _userData;
        public UserDetailModel UserData
        {
            get => _userData;
            set
            {
                _userData = value;
                OnPropertyChanged();

                LoadRights();
            }
        }

        private string _oldPassword;
        [LocalizationString("UserDetail_OldPassword/Header")]
        public string OldPassword
        {
            get => _oldPassword;
            set
            {
                _oldPassword = value;
                OnPropertyChanged();
            }
        }

        private List<Right> _rights;
        public List<Right> Rights
        {
            get => _rights;
            set
            {
                _rights = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand UpdateUserCommand { get; set; }
        public ICommand UpdatePasswordCommand { get; set; }
        public ICommand UpdateRightsCommand { get; set; }

        #endregion

        #region Interface

        public UserDetailViewModel(IMessenger messenger, DbUser dbUser) : base(messenger)
        {
            _dbUser = dbUser;

            UpdateUserCommand = new RelayCommand(UpdateUser);
            UpdatePasswordCommand = new RelayCommand(UpdatePassword);
            UpdateRightsCommand = new RelayCommand(UpdateRights); // todo not finished

            _messenger.Register<SelectedUserChangedMsg>(SelectedUserChanged);
            _messenger.Register<UserUpdatedMsg>(UserUpdated);
        }

        #endregion

        #region Private

        private void UserUpdated(UserUpdatedMsg obj)
        {
            if (obj.UserId != UserData.ID)
                return;

            UserDetailModel userData = null;
            
            RunUnderBusyDialog(() =>userData = _dbUser.GetUserByID(UserData.ID));
            
            UserData = userData;
        }

        private void UpdateUser(object obj) => _messenger.Send(new UpdateUserMsg(UserData.ID, allowUpdateRights: false));

        private void SelectedUserChanged(SelectedUserChangedMsg obj) => UserData = obj.SelectedUser;

        private void UpdateRights(object obj)
        {
            _messenger.Send(new UpdateUserMsg(UserData.ID, allowUpdateUserData: false));
            UserUpdated(new(UserData.ID));
        }

        private void UpdatePassword(object obj)
        {
            ClearValidationMessage();

            if (CheckInsertedPasswords() && CheckOldPassword())
            {
                UserData.DoBeforeCheck();
                _dbUser.EditUser(UserData);

                OldPassword = string.Empty;
                UserUpdated(new(UserData.ID));
            }
        }

        private bool CheckInsertedPasswords()
        {
            if (CheckInsertedPasswords(OldPassword, Localization.GetLocalizedString(AttributeReader.GetPropertyLocalizationStringValue(this, nameof(OldPassword)))) &&
                CheckInsertedPasswords(UserData.NewPassword, Localization.GetLocalizedString(AttributeReader.GetPropertyLocalizationStringValue(UserData, nameof(UserData.NewPassword)))) &&
                CheckInsertedPasswords(UserData.NewPasswordAgain, Localization.GetLocalizedString(AttributeReader.GetPropertyLocalizationStringValue(UserData, nameof(UserData.NewPasswordAgain)))))
                return true;

            return false;
        }

        private bool CheckInsertedPasswords(string password, string passwordLocalization)
        {
            if (string.IsNullOrEmpty(password))
            {
                string header = passwordLocalization;
                string message = Localization.GetLocalizedString(LocalizationStrings.NotInserted);

                SetValidationMessage(header, message);
                return false;
            }

            return true;
        }

        private bool CheckOldPassword()
        {
            if (!Crypto.Decrypt(UserData.Salt, UserData.Hash, OldPassword))
            {
                string header = Localization.GetLocalizedString(AttributeReader.GetPropertyLocalizationStringValue(this, nameof(OldPassword)));
                string message = Localization.GetLocalizedString(LocalizationStrings.PasswordIncorrect);

                SetValidationMessage(header, message);
                return false;
            }

            return true;
        }

        private void SetValidationMessage(string header, string message)
        {
            ErrorHeader = header;
            ErrorMessage = message;
        }

        private void ClearValidationMessage() => SetValidationMessage(null, null);

        private void LoadRights()
        {
            Rights = UserData.Rights.DecodeRights();
        }

        #endregion
    }
}
