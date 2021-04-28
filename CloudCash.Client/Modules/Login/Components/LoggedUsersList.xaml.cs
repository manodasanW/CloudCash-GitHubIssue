using CloudCash.BL.DTOs.Users;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;

namespace CloudCash.Client.Modules.Login.Components
{
    public sealed partial class LoggedUsersList : UserControl
    {
        public LoggedUsersList()
        {
            InitializeComponent();
        }

        public LoggedUsersList(List<UserDetailModel> users) : this()
        {
            UsersList.ItemsSource = users;
            UsersList.SelectedIndex = 0;
        }

        public UserDetailModel GetSelectedUser() => (UserDetailModel)UsersList.SelectedItem ?? null;
    }
}
