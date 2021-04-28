namespace CloudCash.Client.Modules.Settings.Messages
{
    public record UserUpdatedMsg
    {
        public long UserId;

        public UserUpdatedMsg(long userId) => UserId = userId;
    }
}
