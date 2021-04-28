using CloudCash.BL.DbAccess;
using CloudCash.Client.Modules.AppWindow.ViewModels;
using CloudCash.Client.Modules.MainPage.ViewModels;
using CloudCash.Client.Modules.MenuItem.ViewModels;
using CloudCash.Client.Modules.Payment.ViewModels;
using CloudCash.Client.Modules.Product.ViewModels;
using CloudCash.Client.Modules.ProductCategory.ViewModels;
using CloudCash.Client.Modules.ProductSelector.ViewModels;
using CloudCash.Client.Modules.Settings.ViewModels;
using CloudCash.Client.Modules.Table.ViewModels;
using CloudCash.Client.Modules.TableCategory.ViewModels;
using CloudCash.Client.Modules.TableInfo.ViewModels;
using CloudCash.Client.Modules.TablesView.ViewModels;
using CloudCash.Client.Modules.User.ViewModels;
using CloudCash.Common.Functions;
using CloudCash.DAL.Factories;

namespace CloudCash.Client.MVVM
{
    public class VMLocator
    {
        private readonly Messenger _messenger = new();

        private readonly DbCard _dbCard = new(new DbContextFactory());
        private readonly DbCustomer _dbCustomer = new(new DbContextFactory());
        private readonly DbExpenseIncome _dbExpenseIncome = new(new DbContextFactory());
        private readonly DbProduct _dbProduct = new(new DbContextFactory());
        private readonly DbProductCategory _dbProductCategory = new(new DbContextFactory());
        private readonly DbReservation _dbReservations = new(new DbContextFactory());
        private readonly DbSell _dbSell = new(new DbContextFactory());
        private readonly DbTable _dbTable = new(new DbContextFactory());
        private readonly DbTableCategory _dbTableCategory = new(new DbContextFactory());
        private readonly DbTableInfo _dbTableInfo = new(new DbContextFactory());
        public readonly DbPayment _dbPayment = new(new DbContextFactory());
        public readonly DbShift _dbShift = new(new DbContextFactory());
        public readonly DbUser _dbUser = new(new DbContextFactory());
        public readonly DbUserLog _dbUserLog = new(new DbContextFactory());

        private MenuViewModel _menuVM;
        private PaymentViewModel _paymentVM;
        private TableViewModel _tableVM;
        private ProductSelectorViewModel _prouctSelectorVM;
        private SettingsViewModel _settingsVM;
        private TablesViewViewModel _tablesViewVM;
        private MainPageViewModel _mainPageVM;
        private SettingsAppViewModel _settingsAppVM;
        private SettingsTableCategoriesViewModel _settingsTableCategoriesVM;
        private SettingsTableInfosViewModel _settingsTableInfosVM;
        private SettingsUsersViewModel _settingsUsersVM;
        private TableInfoDetailViewModel _tableInfoDetailVM;
        private TableCategoryDetailViewModel _tableCategoryDetailVM;
        private UserDetailViewModel _userDetailVM;
        private AppWindowViewModel _appWindowVM;
        private SettingsProductCategoriesViewModel _settingsProductCategoriesVM;
        private ProductCategoryDetailViewModel _productCategoryDetailVM;
        private SettingsProductsViewModel _settingsProductsVM;
        private ProductDetailViewModel _productDetailVM;
        private ProductSelectorViewModel _productSelectorVM;

        public MenuViewModel MenuVM => _menuVM ??= new(_messenger);
        public PaymentViewModel PaymentVM => _paymentVM ??= new(_messenger, _dbPayment, _dbSell, _dbCustomer, _dbTable, _dbUser, _dbCard, _dbProduct);
        public TableViewModel TableVM => _tableVM ??= new(_messenger, _dbTable, _dbTableCategory, _dbTableInfo, _dbSell, _dbPayment, _dbCustomer, _dbProduct, _dbProductCategory, _dbReservations, _dbCard, _dbUser);
        public ProductSelectorViewModel ProuctSelectorVM => _prouctSelectorVM ??= new(_messenger, _dbProduct, _dbProductCategory);
        public SettingsViewModel SettingsVM => _settingsVM ??= new(_messenger, _dbUser);
        public TablesViewViewModel TablesViewVM => _tablesViewVM ??= new(_messenger, _dbTable, _dbTableInfo, _dbTableCategory, _dbSell);
        public MainPageViewModel MainPageVM => _mainPageVM ??= new(_messenger);
        public SettingsAppViewModel SettingsAppVM => _settingsAppVM ??= new(_messenger);
        public SettingsTableCategoriesViewModel SettingsTableCategoriesVM => _settingsTableCategoriesVM ??= new(_messenger, _dbTableCategory);
        public SettingsTableInfosViewModel SettingsTableInfosVM => _settingsTableInfosVM ??= new(_messenger, _dbTableInfo, _dbTableCategory);
        public SettingsUsersViewModel SettingsUsersVM => _settingsUsersVM ??= new(_messenger, _dbUser);
        public TableInfoDetailViewModel TableInfoDetailVM => _tableInfoDetailVM ??= new(_messenger, _dbTableInfo);
        public TableCategoryDetailViewModel TableCategoryDetailVM => _tableCategoryDetailVM ??= new(_messenger, _dbTableCategory, _dbTableInfo);
        public UserDetailViewModel UserDetailVM => _userDetailVM ??= new(_messenger, _dbUser);
        public AppWindowViewModel AppWindowVM => _appWindowVM ??= new(_messenger);
        public SettingsProductCategoriesViewModel SettingsProductCategoriesVM => _settingsProductCategoriesVM ??= new(_messenger, _dbProductCategory);
        public ProductCategoryDetailViewModel ProductCategoryDetailVM => _productCategoryDetailVM ??= new(_messenger, _dbProductCategory, _dbProduct);
        public SettingsProductsViewModel SettingsProductsVM => _settingsProductsVM ??= new(_messenger, _dbProduct, _dbProductCategory);
        public ProductDetailViewModel ProductDetailVM => _productDetailVM ??= new(_messenger, _dbProduct);
        public ProductSelectorViewModel ProductSelectorVM => _productSelectorVM ??= new(_messenger, _dbProduct, _dbProductCategory);

        public Messenger GetMessenger() => _messenger;
    }
}
