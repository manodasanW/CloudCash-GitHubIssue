using CloudCash.Client.Modules.MenuItem.Enums;
using CloudCash.Client.Modules.MenuItem.Items.Base;
using CloudCash.Common.Functions;

namespace CloudCash.Client.Modules.MenuItem.Items
{
    public class AppCloseMenuItem : UnfinishedMenuItem
    {
        public AppCloseMenuItem(MenuItemSize size, Messenger messenger) : base("AppClose", "AppClose", 0, size, messenger) { }

        protected override async void Navigation(object obj) => await App.Current.LoggedUsersManager.LogOutAndCloseApp();
    }
}
