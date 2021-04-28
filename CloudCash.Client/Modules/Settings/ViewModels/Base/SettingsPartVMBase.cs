using CloudCash.Common.Enums;
using CloudCash.Common.Functions;
using CloudCash.Common.MVVM;
using CloudCash.Interface.Common;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Windows.Input;

namespace CloudCash.Client.Modules.Settings.ViewModels.Base
{
    public abstract class SettingsPartVMBase : ViewModelBase
    {
        protected Frame _navDetailFrame;

        public string Name { get; }

        public Type NavigateToType { get; private set; }

        public ICommand FrameLoadedCommand { get; protected set; }

        public SettingsPartVMBase(LocalizationStrings nameReference, Type navigateToType, IMessenger messenger) : base(messenger)
        {
            NavigateToType = navigateToType;

            Name = Localization.GetLocalizedString(nameReference);

            FrameLoadedCommand = new RelayCommand(FrameLoaded);
        }

        protected void FrameLoaded(object obj)
        {
            _navDetailFrame = obj.CheckType<Frame>();
        }
    }
}
