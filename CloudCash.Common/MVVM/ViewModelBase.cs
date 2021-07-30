using CloudCash.Common.Functions;
using CloudCash.Interface.Common;
using Microsoft.UI.Xaml.Controls;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CloudCash.Common.MVVM
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        protected IMessenger _messenger;
        protected Page _page;

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            private set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        private string _errorHeader;
        public string ErrorHeader
        {
            get => _errorHeader;
            set
            {
                _errorHeader = value;
                OnPropertyChanged();
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public ICommand PageLoadedCommand { get; protected set; }

        public ViewModelBase(IMessenger messenger)
        {
            messenger.CheckNull();

            _messenger = messenger;

            PageLoadedCommand = new RelayCommand(PageLoaded);
        }

        public async Task RunUnderBusyDialog(Func<Task> fceToDo)
        {
            try
            {
                IsBusy = true;
                await Task.Run(fceToDo);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void RunUnderBusyDialog(Action fceToDo)
        {
            try
            {
                IsBusy = true;
                fceToDo.Invoke();
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected virtual void PageLoaded(object obj) => _page = obj.CheckType<Page>();

        public void HideBusyDialog() => IsBusy = false;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string caller = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }
}
