using CloudCash.Client.Modules.Login.Messages;
using CloudCash.Client.Modules.MenuItem.Enums;
using CloudCash.Client.Modules.MenuItem.Items.Base;
using CloudCash.Client.Modules.Payment.Messages;
using CloudCash.Interface.Common;

namespace CloudCash.Client.Modules.MenuItem.Items
{
    public class ShiftsMenuItem : UnfinishedMenuItem
    {
        private uint _shiftCashValue;

        public ShiftsMenuItem(MenuItemSize size, IMessenger messenger) : base("Shift", "Shifts", 2, size, messenger)
        {
            LoggedUserUpdated(null);
            GetShiftData();

            _messenger.Register<PaymentCompletedMsg>(PaymentCompleted);
        }

        private async void GetShiftData()
        {
            _shiftCashValue = await App.Current.ShiftsManager?.GetShiftCash();
            UpdateShiftCashValue();

            WidgetLine2Text = $"Směna otevřena: {App.Current.ShiftsManager?.GetShiftOpenTime() ?? string.Empty}";
            OnPropertyChanged(nameof(WidgetLine2Text));
        }

        private void UpdateShiftCashValue()
        {
            WidgetLine1Text = $"Hodnota kasy: {_shiftCashValue} Kč";
            OnPropertyChanged(nameof(WidgetLine1Text));
        }

        private void PaymentCompleted(PaymentCompletedMsg obj)
        {
            _shiftCashValue += obj.PaidCash;
            UpdateShiftCashValue();
        }

        protected override void LoggedUserUpdated(LoggedUsersUpdatedMsg obj) => IsVisible = App.Current.LoggedUsersManager?.IsSomeoneLogged ?? false;

        protected override void Navigation(object obj)
        {
            throw new System.NotImplementedException();
        }
    }
}
