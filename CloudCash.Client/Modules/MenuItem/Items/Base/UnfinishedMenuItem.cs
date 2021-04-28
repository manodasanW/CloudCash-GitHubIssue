using CloudCash.Client.Modules.Login.Messages;
using CloudCash.Client.Modules.MenuItem.Enums;
using CloudCash.Client.Modules.MenuItem.Messages;
using CloudCash.Common.Functions;
using CloudCash.Common.MVVM;
using CloudCash.Interface.Common;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CloudCash.Client.Modules.MenuItem.Items.Base
{
    public abstract class UnfinishedMenuItem : ViewModelBase, INotifyPropertyChanged
    {
        public string Icon { get; }

        public string Title { get; }

        public int WidgetLinesCount { get; }

        public string WidgetLine1Text { get; protected set; }

        public string WidgetLine2Text { get; protected set; }

        public MenuItemSize Size { get; }

        public bool IsWidgetTypeSize => Size != MenuItemSize.List;

        public bool IsIconSize => Size == MenuItemSize.Icon;

        private bool _isVisible;
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (value == _isVisible)
                    return;

                _isVisible = value;
                OnPropertyChanged();
                _messenger.Send(new UpdateMenuItemsMsg());
            }
        }

        public ICommand NavigationCommand { get; set; }

        public UnfinishedMenuItem(string icon, string titleReference, int widgetLinesCount, MenuItemSize size, IMessenger messenger) : base(messenger)
        {
            Icon = icon;
            WidgetLinesCount = widgetLinesCount;
            Size = size;
            IsVisible = true;

            NavigationCommand = new RelayCommand(Navigation);

            _messenger.Register<UpdateMenuItemMsg>(UpdateMenuItem);
            _messenger.Register<LoggedUsersUpdatedMsg>(LoggedUserUpdated);

            Title = Localization.GetLocalizedStringForMenuItem(titleReference);

            CheckParameterCombination();
        }

        protected abstract void Navigation(object obj);

        private void CheckParameterCombination()
        {
            Icon.CheckNull(nameof(Icon));
            Title.CheckNull(nameof(Title));

            //if (Size == MenuItemSize.Widget && WidgetLinesCount == 0)
                //throw new NotSupportedException();
        }

        protected virtual void LoggedUserUpdated(LoggedUsersUpdatedMsg obj)
        { }

        private void UpdateMenuItem(UpdateMenuItemMsg obj)
        {

        }

        public string GetWidgetLines()
        {
            throw new NotImplementedException("Function is not implemented.");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string caller = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
    }
}
