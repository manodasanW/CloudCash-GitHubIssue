using CloudCash.Client.Modules.Settings.ViewModels.Base;
using CloudCash.Client.Modules.Settings.Views;
using CloudCash.Common.Attributes;
using CloudCash.Common.Enums;
using CloudCash.Common.Functions;
using CloudCash.Interface.Common;

namespace CloudCash.Client.Modules.Settings.ViewModels
{
    public class SettingsAppViewModel : SettingsPartVMBase
    {
        public string DialogCategoryName { get; } = Localization.GetLocalizedString(LocalizationStrings.DialogCategoryName);
        public string PasswordCategoryName { get; } = Localization.GetLocalizedString(LocalizationStrings.PasswordCategoryName);

        [LocalizationString("Settings/SettingsApp/ShowConfirmDialogForLogOut/Header")]
        public bool ShowConfirmDialogForLogOut
        {
            get => Common.Functions.Settings.ReadSettingsValue<bool>();
            set => Common.Functions.Settings.SaveSettingValue(value);
        }

        [LocalizationString("Settings/SettingsApp/ShowInfoDialogAboutLogOutAllUsers/Header")]
        public bool ShowInfoDialogAboutLogOutAllUsers
        {
            get => Common.Functions.Settings.ReadSettingsValue<bool>();
            set => Common.Functions.Settings.SaveSettingValue(value);
        }

        [LocalizationString("Settings/SettingsApp/ShowPasswordForLogin/Header")]
        public bool ShowPasswordForLogin
        {
            get => Common.Functions.Settings.ReadSettingsValue<bool>();
            set => Common.Functions.Settings.SaveSettingValue(value);
        }

        public SettingsAppViewModel(IMessenger messenger) : base(LocalizationStrings.SettingsApp, typeof(SettingsApp), messenger) { }
    }
}
