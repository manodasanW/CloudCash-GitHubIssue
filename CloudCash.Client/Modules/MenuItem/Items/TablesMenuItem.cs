using CloudCash.Client.Modules.Login.Messages;
using CloudCash.Client.Modules.MainPage.Messages;
using CloudCash.Client.Modules.MenuItem.Enums;
using CloudCash.Client.Modules.MenuItem.Items.Base;
using CloudCash.Interface.Common;

namespace CloudCash.Client.Modules.MenuItem.Items
{
    public class TablesMenuItem : UnfinishedMenuItem
    {
        public TablesMenuItem(MenuItemSize size, IMessenger messenger) : base("Table", "Tables", 2, size, messenger) => LoggedUserUpdated(null);

        protected override void LoggedUserUpdated(LoggedUsersUpdatedMsg obj) => IsVisible = App.Current.LoggedUsersManager?.IsSomeoneLogged ?? false;

        protected override void Navigation(object obj) => _messenger.Send(new NavigateToMsg(typeof(TablesView.Views.TablesView)));
    }
}
