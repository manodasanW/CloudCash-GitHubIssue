using CloudCash.BL.DTOs.ProductCategories;
using CloudCash.Common.MVVM;
using CloudCash.Interface.Common;
using System.Collections.ObjectModel;

namespace CloudCash.Client.Modules.ProductCategory.ViewModels
{
    public class ProductCategoryEditViewModel : UserControlViewModelBase<ProductCategoryModel>
    {
        private ObservableCollection<ProductCategoryModel> _productCategories;

        public ProductCategoryEditViewModel(IMessenger messenger, ObservableCollection<ProductCategoryModel> productCategories, ProductCategoryModel data = null) : base(messenger, data)
        {
            _productCategories = productCategories;
        }

        public override void CheckData()
        {
            Data.CheckValues(_productCategories);
        }
    }
}
