using CloudCash.BL.DTOs.ProductCategories;

namespace CloudCash.Client.Modules.Settings.Messages
{
    public record SelectedProductCategoryChangedMsg
    {
        public ProductCategoryModel SelectedProductCategory;

        public SelectedProductCategoryChangedMsg(ProductCategoryModel productCategoryModel) => SelectedProductCategory = productCategoryModel;
    }
}
