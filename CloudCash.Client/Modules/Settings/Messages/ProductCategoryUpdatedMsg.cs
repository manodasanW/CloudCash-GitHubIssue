namespace CloudCash.Client.Modules.Settings.Messages
{
    public record ProductCategoryUpdatedMsg
    {
        public long ProductCategoryId;

        public ProductCategoryUpdatedMsg(long productCategoryId) => ProductCategoryId = productCategoryId;
    }
}
