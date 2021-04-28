using CloudCash.BL.DTOs.TableCategories;

namespace CloudCash.Client.Modules.TableCategory.Messages
{
    public record UpdateTableCategoryMsg
    {
        public TableCategoryModel TableCategoryData;

        public UpdateTableCategoryMsg(TableCategoryModel tableCategoryData) => TableCategoryData = tableCategoryData;
    }
}
