using CloudCash.BL.DbAccess;
using CloudCash.BL.DTOs.ProductCategories;
using CloudCash.BL.DTOs.Products;
using CloudCash.Client.Modules.Product.Messages;
using CloudCash.Client.Modules.Product.ViewModels;
using CloudCash.Client.Modules.Product.Views;
using CloudCash.Client.Modules.Settings.Messages;
using CloudCash.Client.Modules.Settings.ViewModels.Base;
using CloudCash.Client.Modules.Settings.Views;
using CloudCash.Common.Enums;
using CloudCash.Common.Functions;
using CloudCash.Common.MVVM;
using CloudCash.Interface.Common;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CloudCash.Client.Modules.Settings.ViewModels
{
    public class SettingsProductsViewModel : SettingsPartVMBase
    {
        private readonly DbProductCategory _dbProductCategory;
        private readonly DbProduct _dbProduct;
        private ObservableCollection<ProductCategoryModel> _productCategories = new();
        private ObservableCollection<ProductListModel> _products = new();

        #region Props

        public CollectionViewSource GroupedProducts { get; set; } = new();

        private bool _errorMessageVisible;
        public bool ErrorMessageVisible
        {
            get => _errorMessageVisible;
            set
            {
                _errorMessageVisible = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand AddProductCommand { get; }
        public ICommand EditProductCommand { get; }
        public ICommand RemoveProductCommand { get; }
        public ICommand SelectedProductChangedCommand { get; }

        #endregion

        #region Interface


        public SettingsProductsViewModel(IMessenger messenger, DbProduct dbProduct, DbProductCategory dbProductCategory) : base(LocalizationStrings.SettingsProducts, typeof(SettingsProducts), messenger)
        {
            _dbProduct = dbProduct;
            _dbProductCategory = dbProductCategory;

            AddProductCommand = new RelayCommand(AddProduct);
            EditProductCommand = new RelayCommand(EditProduct);
            RemoveProductCommand = new RelayCommand(RemoveProduct);
            SelectedProductChangedCommand = new RelayCommand(SelectedProductChanged);

            _messenger.Register<ProductsUpdatedMsg>(ProductsUpdated);
            _messenger.Register<UpdateProductMsg>(UpdateProduct);

            GroupedProducts.IsSourceGrouped = true;
        }

        #endregion

        #region Private

        protected override async void PageLoaded(object obj)
        {
            base.PageLoaded(obj);

            await LoadProducts();
            await LoadCategories();
        }

        private async Task LoadCategories()
        {
            await RunUnderBusyDialog(async () => _productCategories = await _dbProductCategory.GetProductCategories());

            ErrorMessageVisible = _productCategories.Count is 0;
        }

        private async Task LoadProducts()
        {
            await RunUnderBusyDialog(async () => _products = await _dbProduct.GetProductsList());
            
            UpdateGroupedView();
        }

        private void UpdateGroupedView()
        {
            GroupedProducts.Source = _products.GroupBy(x => x.Category.Name);
            OnPropertyChanged(nameof(GroupedProducts));
        }

        private ProductDetailModel GetProductById(long id)
        {
            ProductDetailModel res = null;

            RunUnderBusyDialog(() => res = _dbProduct.GetProductByID(id));
            
            return res;
        }

        private async void AddProduct(object obj)
        {
            var vm = new ProductEditViewModel(_messenger, _products, _productCategories);

            var dialog = new Controls.ContentDialog()
            {
                XamlRoot = _page.XamlRoot,
                Content = new Product.Components.ProductEdit()
                {
                    DataContext = vm
                },
                PrimaryButtonText = Localization.GetLocalizedString(LocalizationStrings.AddButton),
                CloseButtonText = Localization.GetLocalizedString(LocalizationStrings.CancelButton),
                DefaultButton = ContentDialogButton.Primary
            };

            if (await dialog.ShowAsync() is ContentDialogResult.Primary)
            {
                _dbProduct.AddProduct(vm.Data);

                await LoadProducts();

                _messenger.Send(new ProductsUpdatedMsg());
            }
        }

        private async void EditProduct(object obj)
        {
            var productToEdit = obj.CheckType<ProductListModel>();

            await EditProductCore(productToEdit.ID);
        }

        private async Task EditProductCore(long productToEditId)
        {
            var vm = new ProductEditViewModel(_messenger, _products, _productCategories, GetProductById(productToEditId));

            var dialog = new Controls.ContentDialog()
            {
                XamlRoot = _page.XamlRoot,
                Content = new Product.Components.ProductEdit()
                {
                    DataContext = vm
                },
                PrimaryButtonText = Localization.GetLocalizedString(LocalizationStrings.EditButton),
                CloseButtonText = Localization.GetLocalizedString(LocalizationStrings.CancelButton),
                DefaultButton = ContentDialogButton.Primary
            };

            if (await dialog.ShowAsync() is ContentDialogResult.Primary)
            {
                _dbProduct.EditProduct(vm.Data);

                _products[_products.IndexOf(_products.First(x => x.ID == productToEditId))] = GetProductById(productToEditId);
                UpdateGroupedView();

                _messenger.Send(new ProductsUpdatedMsg());
                _messenger.Send(new ProductUpdatedMsg(productToEditId));
            }
        }

        private async void RemoveProduct(object obj)
        {
            var productToRemove = obj.CheckType<ProductListModel>();

            var res = await new Controls.ContentDialog()
            {
                XamlRoot = _page.XamlRoot,
                Content = Localization.GetLocalizedString(LocalizationStrings.ContentDialogRemoveText),
                Title = Localization.GetLocalizedString(LocalizationStrings.ContentDialogRemoveHeader),
                PrimaryButtonText = Localization.GetLocalizedString(LocalizationStrings.YesButton),
                SecondaryButtonText = Localization.GetLocalizedString(LocalizationStrings.NoButton),
                DefaultButton = ContentDialogButton.Secondary
            }.ShowAsync();

            if (res is ContentDialogResult.Primary)
            {
                _dbProduct.RemoveProductById(productToRemove.ID);

                _products.Remove(productToRemove);
                UpdateGroupedView();

                _messenger.Send(new ProductsUpdatedMsg());
            }
        }

        private void SelectedProductChanged(object obj)
        {
            if (obj is null)
                return;

            _navDetailFrame.Navigate(typeof(ProductDetail), null, new EntranceNavigationTransitionInfo());
            _messenger.Send(new SelectedProductChangedMsg(GetProductById(obj.CheckType<ProductListModel>().ID)));
        }

        private async void ProductsUpdated(ProductsUpdatedMsg obj) => await LoadCategories();

        private async void UpdateProduct(UpdateProductMsg obj) => await EditProductCore(obj.ProductDataId);

        #endregion
    }
}
