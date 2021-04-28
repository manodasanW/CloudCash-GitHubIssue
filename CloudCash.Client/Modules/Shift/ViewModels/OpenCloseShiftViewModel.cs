using CloudCash.BL.DTOs.Shifts;
using CloudCash.BL.DTOs.Users;
using CloudCash.Common.Enums;
using CloudCash.Common.Functions;
using CloudCash.Common.MVVM;
using CloudCash.Interface.Common;
using System;
using System.Collections.ObjectModel;

namespace CloudCash.Client.Modules.Shift.ViewModels
{
    public class OpenCloseShiftViewModel : UserControlViewModelBase<ShiftDetailModel>
    {
        private ObservableCollection<UserDetailModel> _users;
        public ObservableCollection<UserDetailModel> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }

        private string _header;
        public string Header
        {
            get => _header;
            set
            {
                _header = value;
                OnPropertyChanged();
            }
        }

        private string _userPlaceholder;
        public string UserPlaceholder
        {
            get => _userPlaceholder;
            set
            {
                _userPlaceholder = value;
                OnPropertyChanged();
            }
        }

        public OpenCloseShiftViewModel(IMessenger messenger, ShiftRecordType shiftDialogType) : base(messenger, null)
        {
            if (shiftDialogType is ShiftRecordType.None)
                throw new ArgumentException(nameof(shiftDialogType));

            Data.ShiftRecordType = shiftDialogType;

            if (shiftDialogType is ShiftRecordType.Open)
            {
                Users = new(App.Current.LoggedUsersManager.GetUsersByRight(Right.ShiftOpen));
                Header = Localization.GetLocalizedString(LocalizationStrings.OpenCloseShiftOpenTitle);
                UserPlaceholder = Localization.GetLocalizedString(LocalizationStrings.OpenCloseShiftOpenUserPlaceholder);
            }
            else
            {
                Users = new(App.Current.LoggedUsersManager.GetUsersByRight(Right.ShiftClose));
                Header = Localization.GetLocalizedString(LocalizationStrings.OpenCloseShiftCloseTitle);
                UserPlaceholder = Localization.GetLocalizedString(LocalizationStrings.OpenCloseShiftCloseUserPlaceholder);
            }
        }

        public override void CheckData() => Data.CheckValues();
    }
}
