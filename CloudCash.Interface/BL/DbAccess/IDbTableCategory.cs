using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CloudCash.Interface.BL.DbAccess
{
    public interface IDbTableCategory
    {
        bool AddTableCategory(IListModelBase tableCategoryModel);

        bool RemoveTableCategory(IListModelBase tableCategoryModel);

        bool EditTableCategory(IListModelBase tableCategoryModel);

        IListModelBase GetTableCategoryByName(string name);

        Task<ObservableCollection<IListModelBase>> GetTableCategories();
    }
}
