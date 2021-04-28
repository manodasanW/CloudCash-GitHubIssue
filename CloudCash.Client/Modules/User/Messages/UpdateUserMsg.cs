namespace CloudCash.Client.Modules.User.Messages
{
    public record UpdateUserMsg
    {
        public long UserDataId;
        public bool AllowUpdateUserData;
        public bool AllowUpdateRights;

        public UpdateUserMsg(long userDataId, bool allowUpdateUserData = true, bool allowUpdateRights = true)
        {
            UserDataId = userDataId;
            AllowUpdateUserData = allowUpdateUserData;
            AllowUpdateRights = allowUpdateRights;
        }
    }
}
