using CloudCash.BL.DbAccess;
using CloudCash.BL.DTOs.Users;
using CloudCash.Client.Modules.Login.ViewModels;
using CloudCash.Common.Enums;
using CloudCash.Common.Functions;
using CloudCash.Interface.Common;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;

namespace CloudCash.Client.Modules.Login.Classes
{
    public static class LoginDialogHelper
    {
        public async static Task<UserDetailModel> ShowLoginDialog(IMessenger messenger, DbUser dbUser, XamlRoot xamlRoot, bool showPassword = true)
        {
            var vm = new LoginViewModel(messenger, dbUser, showPassword);

            var dialog = new Controls.ContentDialog()
            {
                XamlRoot = xamlRoot,
                Content = new Components.Login()
                {
                    DataContext = vm
                },
                PrimaryButtonText = "asd",
                CloseButtonText = Localization.GetLocalizedString(LocalizationStrings.CancelButton),
                DefaultButton = ContentDialogButton.Primary
            };

            return await dialog.ShowAsync() is ContentDialogResult.Primary ? vm.InsertedUser : null;
        }
    }
}
