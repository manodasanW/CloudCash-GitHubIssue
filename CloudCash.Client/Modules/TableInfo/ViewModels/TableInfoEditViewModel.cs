using CloudCash.BL.DTOs.TableCategories;
using CloudCash.BL.DTOs.TableInfo;
using CloudCash.Common.MVVM;
using CloudCash.Interface.Common;
using System.Collections.ObjectModel;

namespace CloudCash.Client.Modules.TableInfo.ViewModels
{
    public class TableInfoEditViewModel : UserControlViewModelBase<TableInfoDetailModel>
    {
        private ObservableCollection<TableInfoDetailModel> _tableInfos;

        private ObservableCollection<TableCategoryModel> _tableCategories;
        public ObservableCollection<TableCategoryModel> TableCategories
        {
            get => _tableCategories;
            set
            {
                _tableCategories = value;
                OnPropertyChanged();
            }
        }

        public TableInfoEditViewModel(IMessenger messenger, ObservableCollection<TableInfoDetailModel> tableInfos, ObservableCollection<TableCategoryModel> tableCategories, TableInfoDetailModel data = null) : base(messenger, data)
        {
            _tableInfos = tableInfos;
            TableCategories = tableCategories;
        }

        public override void CheckData()
        {
            Data.CheckValues(_tableInfos);
        }
    }
}
