namespace CloudCash.Client.Modules.Settings.Messages
{
    public record ProductUpdatedMsg
    {
        public long ProductId;

        public ProductUpdatedMsg(long productId) => ProductId = productId;
    }
}
