using CloudCash.Client.Modules.MenuItem.Enums;
using CloudCash.Client.Modules.MenuItem.Factory;
using CloudCash.Client.Modules.MenuItem.Items.Base;
using CloudCash.Client.Modules.MenuItem.Messages;
using CloudCash.Common.MVVM;
using CloudCash.Interface.Common;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace CloudCash.Client.Modules.MenuItem.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        #region Props

        private ObservableCollection<UnfinishedMenuItem> _menuItems = new();
        public ObservableCollection<UnfinishedMenuItem> MenuItems
        {
            get => new(_menuItems.Where(x => x.IsVisible).ToList());
            set
            {
                _menuItems = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand ControlLoadedCommand { get; }

        #endregion

        #region Interface

        public MenuViewModel(IMessenger messenger) : base(messenger)
        {
            ControlLoadedCommand = new RelayCommand(ControlLoaded);

            _messenger.Register<LoadMenuItemsMsg>(LoadMenuItems);
            _messenger.Register<UpdateMenuItemsMsg>(UpdateMenuItems);
        }

        #endregion

        #region Private

        private void ControlLoaded(object obj) => LoadMenuItems(null);

        private void LoadMenuItems(LoadMenuItemsMsg msg)
        {
            if (MenuItems.Count > 0)
                return;

            var allMenuItems = MenuItemFactory.CreateAllMenuItems(_messenger, MenuItemSize.Widget);
            allMenuItems.ForEach(x => _menuItems.Add(x));

            OnPropertyChanged(nameof(MenuItems));
        }

        private void UpdateMenuItems(UpdateMenuItemsMsg obj) => OnPropertyChanged(nameof(MenuItems));

        #endregion
    }
}
