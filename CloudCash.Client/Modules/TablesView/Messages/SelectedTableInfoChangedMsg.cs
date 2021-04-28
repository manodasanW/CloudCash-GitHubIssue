namespace CloudCash.Client.Modules.TablesView.Messages
{
    public record SelectedTableInfoChangedMsg
    {
        public long TableInfoId { get; }

        public bool IsOpen { get => TableId > -1; }

        public long TableId { get; }

        public SelectedTableInfoChangedMsg(long tableInfoId, long tableId)
        {
            TableInfoId = tableInfoId;
            TableId = tableId;
        }
    }
}
