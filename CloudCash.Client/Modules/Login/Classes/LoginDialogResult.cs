using CloudCash.BL.DTOs.Users;
using Microsoft.UI.Xaml.Controls;

namespace CloudCash.Client.Modules.Login.Classes
{
    public class LoginDialogResult
    {
        public ContentDialogResult DialogResult { get; }
        public UserDetailModel LoggedUser { get; }

        public LoginDialogResult(ContentDialogResult dialogResult, UserDetailModel loggedUser)
        {
            DialogResult = dialogResult;
            LoggedUser = loggedUser;
        }
    }
}
