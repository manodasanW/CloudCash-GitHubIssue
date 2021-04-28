using CloudCash.Client.Modules.Login.Messages;
using CloudCash.Client.Modules.MenuItem.Enums;
using CloudCash.Client.Modules.MenuItem.Items.Base;
using CloudCash.Common.Enums;
using CloudCash.Interface.Common;

namespace CloudCash.Client.Modules.MenuItem.Items
{
    public class UsersMenuItem : UnfinishedMenuItem
    {
        public UsersMenuItem(MenuItemSize size, IMessenger messenger) : base("Users", "Users", 2, size, messenger) => LoggedUserUpdated(null);

        protected override void LoggedUserUpdated(LoggedUsersUpdatedMsg obj) => IsVisible = (App.Current.LoggedUsersManager?.GetSummaryUsersRights().HasFlag(Right.BasicCommands) ?? false) && false; // todo

        protected override void Navigation(object obj)
        {
            throw new System.NotImplementedException();
        }
    }
}
