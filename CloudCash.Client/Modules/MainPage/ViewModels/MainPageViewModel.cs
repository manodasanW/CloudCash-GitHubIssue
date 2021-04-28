using CloudCash.Client.Modules.MainPage.Messages;
using CloudCash.Common.Functions;
using CloudCash.Common.MVVM;
using CloudCash.Interface.Common;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Windows.Input;

namespace CloudCash.Client.Modules.MainPage.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private Frame _navFrame;
        private Type _currentPage = typeof(TablesView.Views.TablesView);

        public string AppName { get; }

        private bool _showMenu = true;
        public bool ShowMenu
        {
            get => _showMenu;
            set
            {
                _showMenu = value;
                OnPropertyChanged();
            }
        }

        public ICommand SwitchMenuCommand { get; }
        public ICommand FrameLoadedCommand { get; }

        public MainPageViewModel(IMessenger messenger) : base(messenger)
        {
            AppName = Windows.ApplicationModel.Package.Current.DisplayName;

            SwitchMenuCommand = new RelayCommand(SwitchMenu);
            FrameLoadedCommand = new RelayCommand(FrameLoaded);

            _messenger.Register<NavigateToMsg>(NavigateTo);
            _messenger.Register<NavigateBackMsg>(NavigateBack);
        }

        private void SwitchMenu(object obj) => ShowMenu = !ShowMenu;

        private void NavigateTo(NavigateToMsg obj)
        {
            if (obj.NavigateToType == _currentPage)
                return;

            _currentPage = obj.NavigateToType;
            _navFrame.Navigate(_currentPage, null, new EntranceNavigationTransitionInfo());
        }

        private void NavigateBack(NavigateBackMsg obj)
        {
            _currentPage = _navFrame.BackStack[0].SourcePageType;
            
            if (_navFrame.CanGoBack)
                _navFrame.GoBack();
            else
                Application.Current.Exit();
        }

        private void FrameLoaded(object obj)
        {
            _navFrame = obj.CheckType<Frame>();

            _navFrame.Navigate(_currentPage);
        }
    }
}
