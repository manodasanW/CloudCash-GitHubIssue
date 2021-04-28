using CloudCash.BL.DbAccess;
using CloudCash.BL.DTOs.Products;
using CloudCash.Client.Modules.Product.Messages;
using CloudCash.Client.Modules.Settings.Messages;
using CloudCash.Common.MVVM;
using CloudCash.Interface.Common;
using System.Windows.Input;

namespace CloudCash.Client.Modules.Product.ViewModels
{
    public class ProductDetailViewModel : ViewModelBase
    {
        private readonly DbProduct _dbProduct;

        #region Props

        private ProductDetailModel _productData;
        public ProductDetailModel ProductData
        {
            get => _productData;
            set
            {
                _productData = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand UpdateProductCommand { get; set; }

        #endregion

        #region Interface

        public ProductDetailViewModel(IMessenger messenger, DbProduct dbProduct) : base(messenger)
        {
            _dbProduct = dbProduct;

            UpdateProductCommand = new RelayCommand(UpdateProduct);

            _messenger.Register<SelectedProductChangedMsg>(SelectedProductChanged);
            _messenger.Register<ProductUpdatedMsg>(ProductUpdated);
            _messenger.Register<ProductCategoryUpdatedMsg>(ProductCategoryUpdated);
        }

        #endregion

        #region Private

        private void ProductCategoryUpdated(ProductCategoryUpdatedMsg obj)
        {
            if (obj.ProductCategoryId != ProductData.Category.ID)
                return;

            LoadProduct();
        }

        private void ProductUpdated(ProductUpdatedMsg obj)
        {
            if (obj.ProductId != ProductData.ID)
                return;

            LoadProduct();
        }

        private void LoadProduct()
        {
            ProductDetailModel productDetailModel = null;
            
            RunUnderBusyDialog(() => productDetailModel = _dbProduct.GetProductByID(ProductData.ID));

            ProductData = productDetailModel;
        }

        private void UpdateProduct(object obj) => _messenger.Send(new UpdateProductMsg(ProductData.ID));

        private void SelectedProductChanged(SelectedProductChangedMsg obj) => ProductData = obj.SelectedProduct;

        #endregion
    }
}
