using CloudCash.BL.DTOs.Users;

namespace CloudCash.Client.Modules.Settings.Messages
{
    public record SelectedUserChangedMsg
    {
        public UserDetailModel SelectedUser;

        public SelectedUserChangedMsg(UserDetailModel userDetailModel) => SelectedUser = userDetailModel;
    }
}
