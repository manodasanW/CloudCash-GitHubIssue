using CloudCash.Client.Modules.Login.Messages;
using CloudCash.Client.Modules.MenuItem.Enums;
using CloudCash.Client.Modules.MenuItem.Items.Base;
using CloudCash.Client.Modules.Shift.Messages;
using CloudCash.Common.Enums;
using CloudCash.Interface.Common;

namespace CloudCash.Client.Modules.MenuItem.Items
{
    public class ShiftOpenMenuItem : UnfinishedMenuItem
    {
        public ShiftOpenMenuItem(MenuItemSize size, IMessenger messenger) : base("ShiftOpen", "ShiftOpen", 0, size, messenger)
        {
            LoggedUserUpdated(null);

            _messenger.Register<ShiftUpdatedMsg>(ShiftUpdate);
        }

        private void ShiftUpdate(ShiftUpdatedMsg obj) => ShouldBeItemVisible();

        protected override void LoggedUserUpdated(LoggedUsersUpdatedMsg obj) => ShouldBeItemVisible();

        private void ShouldBeItemVisible() => IsVisible = (App.Current.LoggedUsersManager?.GetSummaryUsersRights().HasFlag(Right.ShiftClose | Right.ShiftOpen) ?? false) && (!App.Current.ShiftsManager?.IsShiftOpen ?? false);

        protected override async void Navigation(object obj) => await App.Current.ShiftsManager.OpenShift();
    }
}
