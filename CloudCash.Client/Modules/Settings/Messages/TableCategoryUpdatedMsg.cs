namespace CloudCash.Client.Modules.Settings.Messages
{
    public record TableCategoryUpdatedMsg
    {
        public long TableCategoryId;

        public TableCategoryUpdatedMsg(long tableCategoryId) => TableCategoryId = tableCategoryId;
    }
}
