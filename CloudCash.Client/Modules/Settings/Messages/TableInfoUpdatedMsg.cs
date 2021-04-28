namespace CloudCash.Client.Modules.Settings.Messages
{
    public record TableInfoUpdatedMsg
    {
        public long TableInfoId;

        public TableInfoUpdatedMsg(long tableInfoId) => TableInfoId = tableInfoId;
    }
}
