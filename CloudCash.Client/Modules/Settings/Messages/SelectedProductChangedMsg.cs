using CloudCash.BL.DTOs.Products;

namespace CloudCash.Client.Modules.Settings.Messages
{
    public record SelectedProductChangedMsg
    {
        public ProductDetailModel SelectedProduct;

        public SelectedProductChangedMsg(ProductDetailModel productDetailModel) => SelectedProduct = productDetailModel;
    }
}
