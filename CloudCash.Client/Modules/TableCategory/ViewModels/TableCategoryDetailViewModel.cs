using CloudCash.BL.DbAccess;
using CloudCash.BL.DTOs.TableCategories;
using CloudCash.Client.Modules.Settings.Messages;
using CloudCash.Client.Modules.TableCategory.Messages;
using CloudCash.Common.MVVM;
using CloudCash.Interface.Common;
using System.Windows.Input;

namespace CloudCash.Client.Modules.TableCategory.ViewModels
{
    public class TableCategoryDetailViewModel : ViewModelBase
    {
        private readonly DbTableCategory _dbTableCategory;
        private readonly DbTableInfo _dbTableInfo;

        #region Props

        private TableCategoryModel _tableCategoryData;
        public TableCategoryModel TableCategoryData
        {
            get => _tableCategoryData;
            set
            {
                _tableCategoryData = value;
                OnPropertyChanged();

                UpdateTablesInCategoryCount();
            }
        }

        private int _tableInCategoryCount;
        public int TablesInCategoryCount
        {
            get => _tableInCategoryCount;
            set
            {
                _tableInCategoryCount = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand UpdateTableCategoryCommand { get; }

        #endregion

        #region Interface

        public TableCategoryDetailViewModel(IMessenger messenger, DbTableCategory dbTableCategory, DbTableInfo dbTableInfo) : base(messenger)
        {
            _dbTableCategory = dbTableCategory;
            _dbTableInfo = dbTableInfo;

            UpdateTableCategoryCommand = new RelayCommand(UpdateTableCategory);

            _messenger.Register<SelectedTableCategoryChangedMsg>(SelectedTableCategoryChanged);
            _messenger.Register<TableCategoryUpdatedMsg>(TableCategoryUpdated);
        }

        #endregion

        #region Private

        private void TableCategoryUpdated(TableCategoryUpdatedMsg obj)
        {
            if (obj.TableCategoryId != TableCategoryData.ID)
                return;

            TableCategoryModel data = null;

            RunUnderBusyDialog(() => data = _dbTableCategory.GetTableCategoryById(TableCategoryData.ID));

            TableCategoryData = data;
        }

        private void UpdateTableCategory(object obj) => _messenger.Send(new UpdateTableCategoryMsg(TableCategoryData));

        private void SelectedTableCategoryChanged(SelectedTableCategoryChangedMsg obj) => TableCategoryData = obj.SelectedTableCategory;

        private void UpdateTablesInCategoryCount()
        {
            int count = 0;

            RunUnderBusyDialog(() => count= _dbTableInfo.GetNumberOfTablesInCategory(TableCategoryData.ID));

            TablesInCategoryCount = count;
        }

        #endregion
    }
}
