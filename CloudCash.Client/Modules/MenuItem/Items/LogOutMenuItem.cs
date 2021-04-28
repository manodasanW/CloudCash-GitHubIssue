using CloudCash.Client.Modules.Login.Messages;
using CloudCash.Client.Modules.MenuItem.Enums;
using CloudCash.Client.Modules.MenuItem.Items.Base;
using CloudCash.Interface.Common;

namespace CloudCash.Client.Modules.MenuItem.Items
{
    public class LogOutMenuItem : UnfinishedMenuItem
    {
        public LogOutMenuItem(MenuItemSize size, IMessenger messenger) : base("LogOut", "LogOut", 0, size, messenger) => LoggedUserUpdated(null);

        protected override void LoggedUserUpdated(LoggedUsersUpdatedMsg obj) => IsVisible = App.Current.LoggedUsersManager.IsSomeoneLogged;

        protected override async void Navigation(object obj) => await App.Current.LoggedUsersManager.LogoutUser();
    }
}
