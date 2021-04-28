using CloudCash.Client.Modules.AppWindow.Messages;
using CloudCash.Common.Functions;
using CloudCash.Common.MVVM;
using CloudCash.Interface.Common;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Windows.Input;

namespace CloudCash.Client.Modules.AppWindow.ViewModels
{
    public class AppWindowViewModel : ViewModelBase
    {
        private UIElement _container;
        private Frame _navFrame;

        public ICommand FrameLoadedCommand { get; }

        public AppWindowViewModel(IMessenger messenger) : base(messenger)
        {
            FrameLoadedCommand = new RelayCommand(FrameLoaded);

            _messenger.Register<NavigateToWelcomeMsg>(NavigateToWelcome);
            _messenger.Register<NavigateToMainPageMsg>(NavigateToMainPage);
        }

        private void NavigateTo(Type navigateTo) => _navFrame.Navigate(navigateTo, null, new DrillInNavigationTransitionInfo());

        private void NavigateToMainPage(NavigateToMainPageMsg obj) => NavigateTo(typeof(MainPage.Views.MainPage));

        private void NavigateToWelcome(NavigateToWelcomeMsg obj) => NavigateTo(typeof(Views.WelcomeImage));

        protected override async void PageLoaded(object obj)
        {
            _container = obj.CheckType<UIElement>();

            App.LoadManagers(_container.XamlRoot);

            App.Current.ShiftsManager.LoadCurrentShiftStatus();

            if (_navFrame is not null)
                await App.Current.LoggedUsersManager.ShowLogin(null);
        }

        private async void FrameLoaded(object obj)
        {
            _navFrame = obj.CheckType<Frame>();
            _navFrame.Navigate(typeof(Views.WelcomeImage), null, new DrillInNavigationTransitionInfo());

            if (_container is not null)
            await App.Current.LoggedUsersManager.ShowLogin(null);
        }
    }
}
