using CloudCash.BL.DTOs.Shifts;
using CloudCash.Client.MVVM;
using CloudCash.Client.Modules.Shift.Controls;
using CloudCash.Client.Modules.Shift.Messages;
using CloudCash.Client.Modules.Shift.ViewModels;
using CloudCash.Common.Enums;
using CloudCash.Common.Functions;
using CloudCash.Interface.Common;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace CloudCash.Client.Modules.Shift.Classes
{
    public class ShiftsManager
    {
        private readonly IMessenger _messenger;
        private readonly VMLocator _vmLocator;
        private readonly XamlRoot _xamlRoot;

        private ShiftDetailModel _currentShift;

        public bool IsShiftOpen => _currentShift is not null;

        public ShiftsManager(VMLocator vmLocator, XamlRoot xamlRoot)
        {
            _vmLocator = vmLocator;
            _messenger = vmLocator.GetMessenger();
            _xamlRoot = xamlRoot;
        }

        public void LoadCurrentShiftStatus()
        {
            var shiftRecord = _vmLocator._dbShift.GetLastShiftRecord();

            if (shiftRecord.ShiftRecordType is ShiftRecordType.Open)
                _currentShift = shiftRecord;

            _messenger.Send(new ShiftUpdatedMsg());
        }

        public async Task OpenShift()
        {
            if (await AddShiftRecord(ShiftRecordType.Open))
            {
                LoadCurrentShiftStatus();
                _messenger.Send(new ShiftUpdatedMsg());
            }
        }

        public async Task CloseShift()
        {
            if (await AddShiftRecord(ShiftRecordType.Close))
            {
                _currentShift = null;
                _messenger.Send(new ShiftUpdatedMsg());
            }
        }

        public async Task<uint> GetShiftCash() => _currentShift.CashValue + (uint)(await _vmLocator._dbPayment.GetPaymentsByShiftList(_currentShift)).Sum(x => x.Price);

        public string GetShiftOpenTime() => _currentShift.DateTime.ToString("t");

        private async Task<ShiftDetailModel> ShowOpenCloseShiftDialog(ShiftRecordType shiftDialogType)
        {
            string okButtonText;

            if (shiftDialogType is ShiftRecordType.None)
                throw new ArgumentException(nameof(shiftDialogType));

            if (shiftDialogType is ShiftRecordType.Open)
                okButtonText = Localization.GetLocalizedString(LocalizationStrings.OpenCloseShiftOpenOk);
            else
                okButtonText = Localization.GetLocalizedString(LocalizationStrings.OpenCloseShiftCloseOk);

            var vm = new OpenCloseShiftViewModel(_messenger, shiftDialogType);

            var res = await new Client.Controls.ContentDialog()
            {
                XamlRoot = _xamlRoot,
                Content = new OpenCloseShift()
                {
                    DataContext = vm
                },
                PrimaryButtonText = okButtonText,
                CloseButtonText = Localization.GetLocalizedString(LocalizationStrings.CancelButton),
                DefaultButton = ContentDialogButton.Primary
            }.ShowAsync();

            return res is ContentDialogResult.Primary ? vm.Data : null;
        }

        private async Task<bool> AddShiftRecord(ShiftRecordType shiftRecordType)
        {
            var shiftData = await ShowOpenCloseShiftDialog(shiftRecordType);

            if (shiftData is not null)
            {
                _vmLocator._dbShift.AddShift(shiftData);
                return true;
            }

            return false;
        }
    }
}
