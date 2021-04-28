using CloudCash.Client.Modules.Login.Messages;
using CloudCash.Client.Modules.MenuItem.Enums;
using CloudCash.Client.Modules.MenuItem.Items.Base;
using CloudCash.Common.Enums;
using CloudCash.Interface.Common;

namespace CloudCash.Client.Modules.MenuItem.Items
{
    public class StatisticsMenuItem : UnfinishedMenuItem
    {
        public StatisticsMenuItem(MenuItemSize size, IMessenger messenger) : base("Statistics", "Statistics", 2, size, messenger) => LoggedUserUpdated(null);

        protected override void LoggedUserUpdated(LoggedUsersUpdatedMsg obj) => IsVisible = (App.Current.LoggedUsersManager?.GetSummaryUsersRights().HasFlag(Right.BasicCommands) ?? false) && false; // todo

        protected override void Navigation(object obj)
        {
            throw new System.NotImplementedException();
        }
    }
}
