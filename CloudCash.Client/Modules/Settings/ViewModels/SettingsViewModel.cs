using CloudCash.BL.DbAccess;
using CloudCash.Client.MVVM;
using CloudCash.Client.Modules.Login.Classes;
using CloudCash.Client.Modules.MainPage.Messages;
using CloudCash.Client.Modules.Settings.ViewModels.Base;
using CloudCash.Common.Enums;
using CloudCash.Common.Functions;
using CloudCash.Common.MVVM;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CloudCash.Client.Modules.Settings.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly DbUser _dbUser;
        private Frame _navFrame;

        #region Props

        private List<SettingsPartVMBase> _settingsParts = new();
        public List<SettingsPartVMBase> SettingsParts
        {
            get => _settingsParts;
            set
            {
                _settingsParts = value;
                OnPropertyChanged();
            }
        }

        private bool _settingsPartsVisible = false;

        public bool SettingsPartsVisible
        {
            get => _settingsPartsVisible;
            set
            {
                _settingsPartsVisible = value;
                OnPropertyChanged();
            }
        }


        #endregion

        #region Commands

        public ICommand FrameLoadedCommand { get; }
        public ICommand SelectedSettingsPartChangedCommand { get; }

        #endregion

        #region Interface

        public SettingsViewModel(Messenger messenger, DbUser dbUser) : base(messenger)
        {
            _dbUser = dbUser;

            FrameLoadedCommand = new RelayCommand(FrameLoaded);
            SelectedSettingsPartChangedCommand = new RelayCommand(SelectedSettingsPartChanged);

            LoadParts();
        }

        #endregion

        #region Private

        private async void FrameLoaded(object obj)
        {
            _navFrame = obj.CheckType<Frame>();

            if (await CheckCredentials())
            {
                _navFrame?.Navigate(SettingsParts[0].NavigateToType);
                SettingsPartsVisible = true;
            }
        }

        private async Task<bool> CheckCredentials()
        {
            SettingsPartsVisible = false;

            var res = await LoginDialogHelper.ShowLoginDialog(_messenger, _dbUser, _navFrame.XamlRoot);

            if (res is null || !res.Rights.HasFlag(Right.ShowSettings))
            {
                _messenger.Send(new NavigateBackMsg());
                return false;
            }

            return true;
        }

        private void LoadParts()
        {
            SettingsParts.Add(App.GetVMLocator().SettingsAppVM);
            SettingsParts.Add(App.GetVMLocator().SettingsUsersVM);
            SettingsParts.Add(App.GetVMLocator().SettingsTableCategoriesVM);
            SettingsParts.Add(App.GetVMLocator().SettingsTableInfosVM);
            SettingsParts.Add(App.GetVMLocator().SettingsProductCategoriesVM);
            SettingsParts.Add(App.GetVMLocator().SettingsProductsVM);
        }

        private void SelectedSettingsPartChanged(object obj) => _navFrame.Navigate(obj.CheckType<SettingsPartVMBase>().NavigateToType, null, new EntranceNavigationTransitionInfo());

        #endregion
    }
}
