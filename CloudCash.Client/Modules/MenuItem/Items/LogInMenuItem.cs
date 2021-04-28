using CloudCash.Client.Modules.MenuItem.Enums;
using CloudCash.Client.Modules.MenuItem.Items.Base;
using CloudCash.Interface.Common;

namespace CloudCash.Client.Modules.MenuItem.Items
{
    public class LogInMenuItem : UnfinishedMenuItem
    {
        public LogInMenuItem(MenuItemSize size, IMessenger messenger) : base("LogIn", "LogIn", 0, size, messenger) { }

        protected override async void Navigation(object obj) => await App.Current.LoggedUsersManager.ShowLogin(false);
    }
}
