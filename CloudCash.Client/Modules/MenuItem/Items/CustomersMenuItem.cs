using CloudCash.Client.Modules.Login.Messages;
using CloudCash.Client.Modules.MenuItem.Enums;
using CloudCash.Client.Modules.MenuItem.Items.Base;
using CloudCash.Interface.Common;

namespace CloudCash.Client.Modules.MenuItem.Items
{
    public class CustomersMenuItem : UnfinishedMenuItem
    {
        public CustomersMenuItem(MenuItemSize size, IMessenger messenger) : base("Customers", "Customers", 2, size, messenger) => LoggedUserUpdated(null);

        protected override void LoggedUserUpdated(LoggedUsersUpdatedMsg obj) => IsVisible = App.Current.LoggedUsersManager.IsSomeoneLogged && false; // todo

        protected override void Navigation(object obj)
        {
            throw new System.NotImplementedException();
        }
    }
}
