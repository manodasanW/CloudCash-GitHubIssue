using CloudCash.BL.DbAccess;
using CloudCash.BL.DTOs.ProductCategories;
using CloudCash.BL.DTOs.Products;
using CloudCash.Client.Modules.ProductSelector.Messages;
using CloudCash.Client.Modules.Settings.Messages;
using CloudCash.Common.Functions;
using CloudCash.Common.MVVM;
using CloudCash.Interface.Common;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CloudCash.Client.Modules.ProductSelector.ViewModels
{
    public class ProductSelectorViewModel : ViewModelBase
    {
        private readonly DbProduct _dbProduct;
        private readonly DbProductCategory _dbProductCategory;

        #region Props

        private ObservableCollection<ProductListModel> _products;
        public ObservableCollection<ProductListModel> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ProductCategoryModel> _categories;
        public ObservableCollection<ProductCategoryModel> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand AddItemToSellCommand { get; }
        public ICommand ChangeCategoryCommand { get; }
        public ICommand ControlLoadedCommand { get; }

        #endregion

        #region Interface

        public ProductSelectorViewModel(IMessenger messenger, DbProduct dbProduct, DbProductCategory dbProductCategory) : base(messenger)
        {
            _dbProduct = dbProduct;
            _dbProductCategory = dbProductCategory;

            AddItemToSellCommand = new RelayCommand(AddItemToSell);
            ChangeCategoryCommand = new RelayCommand(ChangeCategory);
            ControlLoadedCommand = new RelayCommand(ControlLoaded);

            _messenger.Register<ProductsUpdatedMsg>(UpdateProducts);
            _messenger.Register<ProductUpdatedMsg>(UpdateProducts);
        }

        #endregion

        #region Private

        private void ControlLoaded(object obj) => LoadCategories(true);

        private async void LoadCategories(bool resetSelectedCategory = false)
        {
            Categories = new(await _dbProductCategory.GetProductCategories());

            if (resetSelectedCategory)
                ChangeCategory(Categories[0]);
        }

        private async void ChangeCategory(object obj)
        {
            ObservableCollection<ProductListModel> products = null;

            await RunUnderBusyDialog(async () => products = new(await _dbProduct.GetProductsByCategoryList(obj.CheckType<ProductCategoryModel>())));

            Products = products;
        }

        private void AddItemToSell(object obj) => _messenger.Send(new AddProductToTableMsg(obj.CheckType<ProductListModel>()));

        private void UpdateProducts(object obj) => LoadCategories();

        #endregion
    }
}
