using CloudCash.BL.DbAccess;
using CloudCash.BL.DTOs.TableCategories;
using CloudCash.BL.DTOs.TableInfo;
using CloudCash.BL.DTOs.Tables;
using CloudCash.Client.Modules.MainPage.Messages;
using CloudCash.Client.Modules.Payment.Messages;
using CloudCash.Client.Modules.Settings.Messages;
using CloudCash.Client.Modules.Table.Messages;
using CloudCash.Common.Functions;
using CloudCash.Common.MVVM;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CloudCash.Client.Modules.TablesView.ViewModels
{
    public class TablesViewViewModel : ViewModelBase
    {
        private readonly DbTable _dbTable;
        private readonly DbTableInfo _dbTableInfo;
        private readonly DbTableCategory _dbTableCategory;
        private readonly DbSell _dbSell;

        #region Props

        private ObservableCollection<TableListModel> _openedTables = new();
        public ObservableCollection<TableListModel> OpenedTables
        {
            get => _openedTables;
            set
            {
                _openedTables = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<TableInfoDetailModel> _tablesInCategory = new();
        public ObservableCollection<TableInfoDetailModel> TablesInCategory
        {
            get => _tablesInCategory;
            set
            {
                _tablesInCategory = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<TableCategoryModel> _categories = new();
        public ObservableCollection<TableCategoryModel> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<TableInfoDetailModel> _tableInfos = new();
        public ObservableCollection<TableInfoDetailModel> TableInfos
        {
            get => _tableInfos;
            set
            {
                _tableInfos = value;
                OnPropertyChanged();
            }
        }

        private TableCategoryModel _selectedCategory;
        public TableCategoryModel SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                CategoryChanged();
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand OnTableClickCommand { get; }

        #endregion

        #region Interface

        public TablesViewViewModel(Messenger messenger, DbTable dbTable, DbTableInfo dbTableInfo, DbTableCategory dbTableCategory, DbSell dbSell) : base(messenger)
        {
            _dbTable = dbTable;
            _dbTableInfo = dbTableInfo;
            _dbTableCategory = dbTableCategory;
            _dbSell = dbSell;

            OnTableClickCommand = new RelayCommand(OnTableClick);

            _messenger.Register<TableInfosUpdatedMsg>(TableInfosUpdated);
            _messenger.Register<PaymentCompletedMsg>(PaymentCompleted);
            _messenger.Register<TableOpenedMsg>(TableOpened);
        }

        #endregion

        #region Private

        protected override async void PageLoaded(object obj)
        {
            base.PageLoaded(obj);

            await LoadTableInfos();
            await LoadCategories();
            await LoadOpenedTables();
        }

        private void OnTableClick(object obj)
        {
            var tableInfo = obj.CheckType<TableInfoDetailModel>();

            _messenger.Send(new NavigateToMsg(typeof(Table.Views.Table)));

            var table = OpenedTables.FirstOrDefault(x => x.TableInfo.ID == tableInfo.ID);

            _messenger.Send(new Messages.SelectedTableInfoChangedMsg(tableInfo.ID, table?.ID ?? -1));
        }

        private async void TableInfosUpdated(TableInfosUpdatedMsg obj)
        {
            await LoadCategories();
            await LoadTableInfos();
            CategoryChanged();
        }

        private void PaymentCompleted(PaymentCompletedMsg obj)
        {
            var table = OpenedTables.FirstOrDefault(x => x.ID == obj.TableInfoId);

            if (obj.TableClosed && table is not null)
                OpenedTables.Remove(table);
        }

        private void TableOpened(TableOpenedMsg obj) => OpenedTables.Add(_dbTable.GetTableById(obj.TableId));

        private async Task LoadOpenedTables() => OpenedTables = await _dbTable.GetOpenedTablesList();

        private async Task LoadCategories()
        {
            ObservableCollection<TableCategoryModel> categories = null;

            await RunUnderBusyDialog(async () => categories = await _dbTableCategory.GetTableCategories());

            Categories = categories;

            if (Categories.Count > 0)
                SelectedCategory = Categories[0];
        }

        private async Task LoadTableInfos()
        {
            ObservableCollection<TableInfoDetailModel> tableInfos = null;

            await RunUnderBusyDialog(async () => tableInfos = await _dbTableInfo.GetTableInfos());

            TableInfos = tableInfos;
        }

        private void CategoryChanged() => TablesInCategory = SelectedCategory is null ? null : new(TableInfos.Where(x => x.Category.ID == SelectedCategory.ID).ToList());

        #endregion
    }
}
