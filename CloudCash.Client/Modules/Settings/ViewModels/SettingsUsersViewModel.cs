using CloudCash.BL.DbAccess;
using CloudCash.BL.DTOs.Users;
using CloudCash.Client.Modules.Settings.Messages;
using CloudCash.Client.Modules.Settings.ViewModels.Base;
using CloudCash.Client.Modules.Settings.Views;
using CloudCash.Client.Modules.User.Messages;
using CloudCash.Client.Modules.User.ViewModels;
using CloudCash.Client.Modules.User.Views;
using CloudCash.Common.Enums;
using CloudCash.Common.Functions;
using CloudCash.Common.MVVM;
using CloudCash.Interface.Common;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CloudCash.Client.Modules.Settings.ViewModels
{
    public class SettingsUsersViewModel : SettingsPartVMBase
    {
        private readonly DbUser _dbUser;

        #region Props

        private ObservableCollection<UserListModel> _users;
        public ObservableCollection<UserListModel> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand AddUserCommand { get; }
        public ICommand EditUserCommand { get; }
        public ICommand RemoveUserCommand { get; }
        public ICommand SelectedUserChangedCommand { get; set; }

        #endregion

        #region Interface

        public SettingsUsersViewModel(IMessenger messenger, DbUser dbUser) : base(LocalizationStrings.SettingsUsers, typeof(SettingsUsers), messenger)
        {
            _dbUser = dbUser;

            AddUserCommand = new RelayCommand(AddUser);
            EditUserCommand = new RelayCommand(EditUser);
            RemoveUserCommand = new RelayCommand(RemoveUser);
            SelectedUserChangedCommand = new RelayCommand(SelectedUserChanged);

            _messenger.Register<UpdateUserMsg>(UpdateUser);
        }

        #endregion

        #region Private

        protected override async void PageLoaded(object obj)
        {
            base.PageLoaded(obj);

            await LoadUsers();
        }

        private async Task LoadUsers()
        {
            ObservableCollection<UserListModel> users = null;
            
            await RunUnderBusyDialog(async () => users = await _dbUser.GetUsersList());
            
            Users = users;
        }

        private UserListModel GetUserListById(long id)
        {
            UserListModel res = null;

            RunUnderBusyDialog(() => res = _dbUser.GetUserListByID(id));
            
            return res;
        }

        private UserDetailModel GetUserDetailById(long id)
        {
            UserDetailModel res = null;

            RunUnderBusyDialog(() => res = _dbUser.GetUserByID(id));
            
            return res;
        }

        private async void AddUser(object obj)
        {
            var vm = new UserEditViewModel(_messenger, _users);

            var dialog = new Controls.ContentDialog()
            {
                XamlRoot = _page.XamlRoot,
                Content = new User.Components.UserEdit()
                {
                    DataContext = vm
                },
                PrimaryButtonText = Localization.GetLocalizedString(LocalizationStrings.AddButton),
                CloseButtonText = Localization.GetLocalizedString(LocalizationStrings.CancelButton),
                DefaultButton = ContentDialogButton.Primary
            };

            if (await dialog.ShowAsync() is ContentDialogResult.Primary)
            {
                _dbUser.AddUser(vm.Data);

                await LoadUsers();

                _messenger.Send(new UsersUpdatedMsg());
            }
        }

        private async void EditUser(object obj)
        {
            var userToEdit = obj.CheckType<UserListModel>();
            await EditUserCore(userToEdit.ID);
        }

        private async Task EditUserCore(long userToEditId, bool allowUpdateUserData = true, bool allowUpdateRights = true)
        {
            var vm = new UserEditViewModel(_messenger, _users, GetUserDetailById(userToEditId), allowUpdateUserData, allowUpdateRights);

            var dialog = new Controls.ContentDialog()
            {
                XamlRoot = _page.XamlRoot,
                Content = new User.Components.UserEdit()
                {
                    DataContext = vm
                },
                PrimaryButtonText = Localization.GetLocalizedString(LocalizationStrings.EditButton),
                CloseButtonText = Localization.GetLocalizedString(LocalizationStrings.CancelButton),
                DefaultButton = ContentDialogButton.Primary
            };

            if (await dialog.ShowAsync() is ContentDialogResult.Primary)
            {
                _dbUser.EditUser(vm.Data);

                _users[_users.IndexOf(_users.First(x => x.ID == userToEditId))] = GetUserListById(userToEditId);

                _messenger.Send(new UsersUpdatedMsg());
                _messenger.Send(new UserUpdatedMsg(userToEditId));
            }
        }

        private async void RemoveUser(object obj)
        {
            var userToRemove = obj.CheckType<UserListModel>();

            var res = await new Controls.ContentDialog()
            {
                XamlRoot = _page.XamlRoot,
                Content = Localization.GetLocalizedString(LocalizationStrings.ContentDialogRemoveText),
                Title = Localization.GetLocalizedString(LocalizationStrings.ContentDialogRemoveHeader),
                PrimaryButtonText = Localization.GetLocalizedString(LocalizationStrings.YesButton),
                SecondaryButtonText = Localization.GetLocalizedString(LocalizationStrings.NoButton),
                DefaultButton = ContentDialogButton.Secondary
            }.ShowAsync();

            if (res is ContentDialogResult.Primary)
            {
                _dbUser.RemoveUserById(userToRemove.ID);

                _users.Remove(userToRemove);

                _messenger.Send(new UsersUpdatedMsg());
            }
        }

        private void SelectedUserChanged(object obj)
        {
            if (obj is null)
                return;

            _navDetailFrame.Navigate(typeof(UserDetail), null, new EntranceNavigationTransitionInfo());
            _messenger.Send(new SelectedUserChangedMsg(GetUserDetailById(obj.CheckType<UserListModel>().ID)));
        }

        private async void UpdateUser(UpdateUserMsg obj) => await EditUserCore(obj.UserDataId, obj.AllowUpdateUserData, obj.AllowUpdateRights);

        #endregion
    }
}
