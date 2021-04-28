using CloudCash.BL.DTOs.TableCategories;
using CloudCash.Common.MVVM;
using CloudCash.Interface.Common;
using System.Collections.ObjectModel;

namespace CloudCash.Client.Modules.TableCategory.ViewModels
{
    public class TableCategoryEditViewModel : UserControlViewModelBase<TableCategoryModel>
    {
        private ObservableCollection<TableCategoryModel> _tableCategories;

        public TableCategoryEditViewModel(IMessenger messenger, ObservableCollection<TableCategoryModel> tableCategories, TableCategoryModel data = null) : base(messenger, data)
        {
            _tableCategories = tableCategories;
        }

        public override void CheckData()
        {
            Data.CheckValues(_tableCategories);
        }
    }
}
