using CloudCash.BL.DbAccess;
using CloudCash.BL.DTOs.TableInfo;
using CloudCash.Client.Modules.Settings.Messages;
using CloudCash.Client.Modules.TableInfo.Messages;
using CloudCash.Common.MVVM;
using CloudCash.Interface.Common;
using System.Windows.Input;

namespace CloudCash.Client.Modules.TableInfo.ViewModels
{
    public class TableInfoDetailViewModel : ViewModelBase
    {
        private readonly DbTableInfo _dbTableInfo;

        #region Props

        private TableInfoDetailModel _tableInfoData;
        public TableInfoDetailModel TableInfoData
        {
            get => _tableInfoData;
            set
            {
                _tableInfoData = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand UpdateTableInfoCommand { get; set; }

        #endregion

        #region Interface

        public TableInfoDetailViewModel(IMessenger messenger, DbTableInfo dbTableInfo) : base(messenger)
        {
            _dbTableInfo = dbTableInfo;

            UpdateTableInfoCommand = new RelayCommand(UpdateTableInfo);

            _messenger.Register<SelectedTableInfoChangedMsg>(SelectedTableInfoChanged);
            _messenger.Register<TableInfoUpdatedMsg>(TableInfoUpdated);
            _messenger.Register<TableCategoryUpdatedMsg>(TableCategoryUpdated);
        }

        #endregion

        #region Private

        private void TableCategoryUpdated(TableCategoryUpdatedMsg obj)
        {
            if (obj.TableCategoryId != TableInfoData.Category.ID)
                return;

            LoadTableInfo();
        }

        private void TableInfoUpdated(TableInfoUpdatedMsg obj)
        {
            if (obj.TableInfoId != TableInfoData.ID)
                return;

            LoadTableInfo();
        }

        private void LoadTableInfo()
        {
            TableInfoDetailModel tableInfoData = null;

            RunUnderBusyDialog(() =>tableInfoData = _dbTableInfo.GetTableInfoByID(TableInfoData.ID));

            TableInfoData = tableInfoData;
        }

        private void UpdateTableInfo(object obj) => _messenger.Send(new UpdateTableInfoMsg(TableInfoData));

        private void SelectedTableInfoChanged(SelectedTableInfoChangedMsg obj) => TableInfoData = obj.SelectedTableInfo;

        #endregion
    }
}
