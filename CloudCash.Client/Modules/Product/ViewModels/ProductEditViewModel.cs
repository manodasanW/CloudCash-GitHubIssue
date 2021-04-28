using CloudCash.BL.DTOs.ProductCategories;
using CloudCash.BL.DTOs.Products;
using CloudCash.Common.Enums;
using CloudCash.Common.Functions;
using CloudCash.Common.MVVM;
using CloudCash.Interface.Common;
using System.Collections.ObjectModel;
using System.Linq;

namespace CloudCash.Client.Modules.Product.ViewModels
{
    public class ProductEditViewModel : UserControlViewModelBase<ProductDetailModel>
    {
        private ObservableCollection<ProductListModel> _products;

        private ObservableCollection<ProductCategoryModel> _productCategories;
        public ObservableCollection<ProductCategoryModel> ProductCategories
        {
            get => _productCategories;
            set
            {
                _productCategories = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<VatLevel> _vatLevels;
        public ObservableCollection<VatLevel> VatLevels
        {
            get => _vatLevels;
            set
            {
                _vatLevels = value;
                OnPropertyChanged();
            }
        }

        public ProductEditViewModel(IMessenger messenger, ObservableCollection<ProductListModel> tableInfos, ObservableCollection<ProductCategoryModel> tableCategories, ProductDetailModel data = null) : base(messenger, data)
        {
            _products = tableInfos;
            ProductCategories = tableCategories;

            VatLevels = new(VatLevel.None.EnumToList().Where(x => x.GetValidity()));
        }

        public override void CheckData() => Data.CheckValues(_products);
    }
}
