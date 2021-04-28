using CloudCash.BL.DTOs.Users;
using CloudCash.Client.MVVM;
using CloudCash.Client.Modules.AppWindow.Messages;
using CloudCash.Client.Modules.Login.Components;
using CloudCash.Client.Modules.Login.Messages;
using CloudCash.Common.Enums;
using CloudCash.Common.Functions;
using CloudCash.Interface.Common;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudCash.Client.Modules.Login.Classes
{
    public class LoggedUsersManager
    {
        private readonly IMessenger _messenger;
        private readonly VMLocator _vmLocator;
        private readonly XamlRoot _xamlRoot;
        private readonly List<UserDetailModel> _loggedUsers = new();

        #region Interface

        public UserDetailModel Getsss => _loggedUsers[0];

        public bool IsSomeoneLogged => _loggedUsers.Count > 0;

        public LoggedUsersManager(VMLocator vmLocator, XamlRoot xamlRoot)
        {
            _vmLocator = vmLocator;
            _messenger = vmLocator.GetMessenger();
            _xamlRoot = xamlRoot;
        }

        public Right GetSummaryUsersRights()
        {
            var res = Right.None;

            foreach (var loggedUser in _loggedUsers)
            {
                res |= loggedUser.Rights;
            }

            return res;
        }

        public List<UserDetailModel> GetUsersByRight(Right right) => _loggedUsers.Where(x => x.Rights.HasFlag(right)).ToList();

        public async Task ShowLogin(bool? showWelcome = null)
        {
            if (showWelcome ?? false)
                _messenger.Send(new NavigateToWelcomeMsg());

            var res = await LoginDialogHelper.ShowLoginDialog(_messenger, _vmLocator._dbUser, _xamlRoot, _vmLocator.SettingsAppVM.ShowPasswordForLogin);

            if (res is not null)
            {
                AddLoggedUser(res);

                if (showWelcome ?? true)
                    _messenger.Send(new NavigateToMainPageMsg());
            }
            else
                if (!IsSomeoneLogged)
                    Application.Current.Exit();
        }

        public async Task LogoutUser()
        {
            var loggedUsersList = new LoggedUsersList(_loggedUsers);

            var selectUserDialog = new ContentDialog
            {
                XamlRoot = _xamlRoot,
                Content = loggedUsersList,
                Title = Localization.GetLocalizedString(LocalizationStrings.ContentDialogLogOutHeader),
                PrimaryButtonText = Localization.GetLocalizedString(LocalizationStrings.LogoutButton),
                CloseButtonText = Localization.GetLocalizedString(LocalizationStrings.CancelButton),
                DefaultButton = ContentDialogButton.Primary
            };

            if (await selectUserDialog.ShowAsync() is not ContentDialogResult.Primary)
                return;

            var selectedUser = loggedUsersList.GetSelectedUser();

            var confirmRes = ContentDialogResult.Primary;

            if (_vmLocator.SettingsAppVM.ShowConfirmDialogForLogOut)
                confirmRes = await new ContentDialog
                {
                    XamlRoot = _xamlRoot,
                    Title = string.Format(Localization.GetLocalizedString(LocalizationStrings.ContentDialogConfirmLogOutHeader), selectedUser.NickName),
                    Content = Localization.GetLocalizedString(LocalizationStrings.ContentDialogLogOutText),
                    PrimaryButtonText = Localization.GetLocalizedString(LocalizationStrings.LogoutButton),
                    CloseButtonText = Localization.GetLocalizedString(LocalizationStrings.CancelButton),
                    DefaultButton = ContentDialogButton.Primary
                }.ShowAsync();

            if (confirmRes is ContentDialogResult.Primary)
            {
                RemoveUser(selectedUser);

                await CheckOtherUsersIfTheyShouldBeLogout();
            }
        }

        public async Task LogOutAndCloseApp()
        {
            var dialogResult = ContentDialogResult.Primary;

            if (_loggedUsers.Count > 1)
            {
                dialogResult = await new ContentDialog
                {
                    XamlRoot = _xamlRoot,
                    Title = Localization.GetLocalizedString(LocalizationStrings.ContentDialogCloseAppLogOutHeader),
                    Content = Localization.GetLocalizedString(LocalizationStrings.ContentDialogCloseAppLogOutText),
                    PrimaryButtonText = Localization.GetLocalizedString(LocalizationStrings.YesButton),
                    CloseButtonText = Localization.GetLocalizedString(LocalizationStrings.NoButton),
                    DefaultButton = ContentDialogButton.Primary
                }.ShowAsync();
            }

            if (dialogResult is ContentDialogResult.Primary)
            {
                RemoveAllUsers();
                Application.Current.Exit();
            }
        }

        #endregion

        #region Private

        private async Task CheckOtherUsersIfTheyShouldBeLogout()
        {
            if (IsSomeoneLogged && !GetSummaryUsersRights().HasFlag(Right.ShiftClose))
            {
                if (_vmLocator.SettingsAppVM.ShowInfoDialogAboutLogOutAllUsers)
                    await new ContentDialog()
                    {
                        XamlRoot = _xamlRoot,
                        Title = Localization.GetLocalizedString(LocalizationStrings.ContentDialogLogOutAllUsersHeader),
                        Content = Localization.GetLocalizedString(LocalizationStrings.ContentDialogLogOutAllUsersText),
                        PrimaryButtonText = Localization.GetLocalizedString(LocalizationStrings.OkButton),
                        DefaultButton = ContentDialogButton.Primary
                    }.ShowAsync();

                while (IsSomeoneLogged)
                    RemoveUser(_loggedUsers[0]);
            }

            await ShowLogin(true);
        }

        private void AddLoggedUser(UserDetailModel user)
        {
            user.CheckNull();

            if (_loggedUsers.FirstOrDefault(x => x.ID == user.ID) is not null)
                throw new NotSupportedException();

            _loggedUsers.Add(user);
            _vmLocator._dbUserLog.AddUserLog(new()
            {
                LogType = UserLogType.LogIn,
                User = user
            });

            _messenger.Send(new LoggedUsersUpdatedMsg());
        }

        private void RemoveAllUsers()
        {
            while (IsSomeoneLogged)
                RemoveUser(_loggedUsers[0]);
        }

        private void RemoveUser(UserDetailModel user)
        {
            user.CheckNull();

            if (_loggedUsers.FirstOrDefault(x => x.ID == user.ID) is null)
                throw new NotSupportedException();

            _loggedUsers.Remove(user);
            _vmLocator._dbUserLog.AddUserLog(new()
            {
                LogType = UserLogType.LogOut,
                User = user
            });

            _messenger.Send(new LoggedUsersUpdatedMsg());
        }

        #endregion
    }
}
