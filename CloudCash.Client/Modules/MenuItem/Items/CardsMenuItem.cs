using CloudCash.Client.Modules.Login.Messages;
using CloudCash.Client.Modules.MenuItem.Enums;
using CloudCash.Client.Modules.MenuItem.Items.Base;
using CloudCash.Common.Enums;
using CloudCash.Common.Functions;

namespace CloudCash.Client.Modules.MenuItem.Items
{
    public class CardsMenuItem : UnfinishedMenuItem
    {
        public CardsMenuItem(MenuItemSize size, Messenger messenger) : base("Cards", "Cards", 2, size, messenger) => LoggedUserUpdated(null);

        protected override void LoggedUserUpdated(LoggedUsersUpdatedMsg obj) => IsVisible = App.Current.LoggedUsersManager.GetSummaryUsersRights().HasFlag(Right.BasicCommands) && false; // todo

        protected override void Navigation(object obj)
        {
            throw new System.NotImplementedException();
        }
    }
}
