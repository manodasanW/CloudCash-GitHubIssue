using CloudCash.BL.DTOs.ProductCategories;

namespace CloudCash.Client.Modules.ProductCategory.Messages
{
    public record UpdateProductCategoryMsg
    {
        public ProductCategoryModel ProductCategoryData;

        public UpdateProductCategoryMsg(ProductCategoryModel productCategoryData) => ProductCategoryData = productCategoryData;
    }
}
