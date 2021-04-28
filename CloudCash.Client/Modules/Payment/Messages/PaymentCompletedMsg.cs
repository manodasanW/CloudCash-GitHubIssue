namespace CloudCash.Client.Modules.Payment.Messages
{
    public record PaymentCompletedMsg
    {
        public long TableInfoId { get; set; }

        public bool TableClosed { get; set; }

        public uint PaidCash { get; set; }

        public PaymentCompletedMsg(long tableInfoId, uint paidCash) : this(tableInfoId, false, paidCash) { }

        public PaymentCompletedMsg(long tableInfoId, bool tableClosed, uint paidCash)
        {
            TableInfoId = tableInfoId;
            TableClosed = tableClosed;
            PaidCash = paidCash;
        }
    }
}
