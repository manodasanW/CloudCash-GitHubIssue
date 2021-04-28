using CloudCash.Client.Modules.Login.Messages;
using CloudCash.Client.Modules.MainPage.Messages;
using CloudCash.Client.Modules.MenuItem.Enums;
using CloudCash.Client.Modules.MenuItem.Items.Base;
using CloudCash.Common.Enums;
using CloudCash.Interface.Common;

namespace CloudCash.Client.Modules.MenuItem.Items
{
    public class SettingsMenuItem : UnfinishedMenuItem
    {
        public SettingsMenuItem(MenuItemSize size, IMessenger messenger) : base("Settings", "Settings", 0, size, messenger) => LoggedUserUpdated(null);

        protected override void Navigation(object obj) => _messenger.Send(new NavigateToMsg(typeof(Settings.Views.Settings)));

        protected override void LoggedUserUpdated(LoggedUsersUpdatedMsg obj) => IsVisible = App.Current.LoggedUsersManager?.GetSummaryUsersRights().HasFlag(Right.ShowSettings) ?? false;
    }
}
