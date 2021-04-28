namespace CloudCash.Client.Modules.Product.Messages
{
    public record UpdateProductMsg
    {
        public long ProductDataId;

        public UpdateProductMsg(long productDataId) => ProductDataId = productDataId;
    }
}
