using CloudCash.BL.DTOs.TableCategories;

namespace CloudCash.Client.Modules.Settings.Messages
{
    public record SelectedTableCategoryChangedMsg
    {
        public TableCategoryModel SelectedTableCategory;

        public SelectedTableCategoryChangedMsg(TableCategoryModel tableCategoryModel) => SelectedTableCategory = tableCategoryModel;
    }
}
