using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CloudCash.Client.Controls
{
    public sealed partial class BusyOverlay : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy", typeof(bool), typeof(BusyOverlay), new PropertyMetadata(null, OnIsBusyChanged));

        private static void OnIsBusyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is bool isBusy)
            {
                var control = (BusyOverlay)d;
                control.IsBusy = isBusy;
            }
        }

        public bool IsBusy
        {
            get => (bool)GetValue(IsBusyProperty);
            set
            {
                SetValue(IsBusyProperty, value);
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string caller = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));

        public BusyOverlay()
        {
            InitializeComponent();
        }
    }
}
