using CloudCash.Client.Modules.MenuItem.Items.Base;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CloudCash.Client.Modules.MenuItem.Views
{
    public sealed partial class MenuItem : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty MenuItemDataProperty =
            DependencyProperty.Register("MenuItemData", typeof(UnfinishedMenuItem), typeof(MenuItem), new PropertyMetadata(null, OnMenuItemDataChanged));

        private static void OnMenuItemDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is UnfinishedMenuItem menuItemAppDefinition)
            {
                var control = (MenuItem)d;
                control.MenuItemData = menuItemAppDefinition;
            }
        }

        public UnfinishedMenuItem MenuItemData
        {
            get => (UnfinishedMenuItem)GetValue(MenuItemDataProperty);
            set
            {
                SetValue(MenuItemDataProperty, value);
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string caller = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));

        public MenuItem()
        {
            InitializeComponent();
        }
    }
}
