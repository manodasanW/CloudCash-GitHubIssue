namespace CloudCash.Client.Modules.Table.Messages
{
    public record TableToPaymentMsg
    {
        public long TableId { get; }

        public TableToPaymentMsg(long tableId) => TableId = tableId;
    }
}
