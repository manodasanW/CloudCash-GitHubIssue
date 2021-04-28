using CloudCash.BL.DbAccess;
using CloudCash.BL.DTOs.ProductCategories;
using CloudCash.Client.Modules.ProductCategory.Messages;
using CloudCash.Client.Modules.Settings.Messages;
using CloudCash.Common.MVVM;
using CloudCash.Interface.Common;
using System.Windows.Input;

namespace CloudCash.Client.Modules.ProductCategory.ViewModels
{
    public class ProductCategoryDetailViewModel : ViewModelBase
    {
        private readonly DbProductCategory _dbProductCategory;
        private readonly DbProduct _dbProduct;

        #region Props

        private ProductCategoryModel _productCategoryData;
        public ProductCategoryModel ProductCategoryData
        {
            get => _productCategoryData;
            set
            {
                _productCategoryData = value;
                OnPropertyChanged();

                UpdateProductsInCategoryCount();
            }
        }

        private int _productsInCategoryCount;
        public int ProductsInCategoryCount
        {
            get => _productsInCategoryCount;
            set
            {
                _productsInCategoryCount = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand UpdateProductCategoryCommand { get; }

        #endregion

        #region Interface

        public ProductCategoryDetailViewModel(IMessenger messenger, DbProductCategory dbProductCategory, DbProduct dbProduct) : base(messenger)
        {
            _dbProductCategory = dbProductCategory;
            _dbProduct = dbProduct;

            UpdateProductCategoryCommand = new RelayCommand(UpdateProductCategory);

            _messenger.Register<SelectedProductCategoryChangedMsg>(SelectedProductCategoryChanged);
            _messenger.Register<ProductCategoryUpdatedMsg>(ProductCategoryUpdated);
        }

        #endregion

        #region Private

        private void ProductCategoryUpdated(ProductCategoryUpdatedMsg obj)
        {
            if (obj.ProductCategoryId != ProductCategoryData.ID)
                return;

            ProductCategoryModel productCategoryData = null;
            
            RunUnderBusyDialog(() =>productCategoryData = _dbProductCategory.GetProductCategoryById(ProductCategoryData.ID));
            
            ProductCategoryData = productCategoryData;
        }

        private void UpdateProductCategory(object obj) => _messenger.Send(new UpdateProductCategoryMsg(ProductCategoryData));

        private void SelectedProductCategoryChanged(SelectedProductCategoryChangedMsg obj) => ProductCategoryData = obj.SelectedProductCategory;

        private void UpdateProductsInCategoryCount()
        {
            int count = 0;

            RunUnderBusyDialog(() => count = _dbProduct.GetNumberOfProductsInCategory(ProductCategoryData.ID));
            
            ProductsInCategoryCount = count;
        }

        #endregion
    }
}
