namespace CloudCash.Client.Modules.Table.Messages
{
    public record TableOpenedMsg
    {
        public long TableId { get; set; }

        public TableOpenedMsg(long tableId) => TableId = tableId;
    }
}
