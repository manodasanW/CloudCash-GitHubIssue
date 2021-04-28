using CloudCash.BL.DbAccess;
using CloudCash.BL.DTOs.ProductCategories;
using CloudCash.Client.Modules.ProductCategory.Messages;
using CloudCash.Client.Modules.ProductCategory.ViewModels;
using CloudCash.Client.Modules.ProductCategory.Views;
using CloudCash.Client.Modules.Settings.Messages;
using CloudCash.Client.Modules.Settings.ViewModels.Base;
using CloudCash.Client.Modules.Settings.Views;
using CloudCash.Common.Enums;
using CloudCash.Common.Functions;
using CloudCash.Common.MVVM;
using CloudCash.Interface.Common;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CloudCash.Client.Modules.Settings.ViewModels
{
    public class SettingsProductCategoriesViewModel : SettingsPartVMBase
    {
        private readonly DbProductCategory _dbProductCategory;

        #region Props

        private ObservableCollection<ProductCategoryModel> _productCategories = new();
        public ObservableCollection<ProductCategoryModel> ProductCategories
        {
            get => _productCategories;
            set
            {
                _productCategories = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand AddProductCategoryCommand { get; }
        public ICommand EditProductCategoryCommand { get; }
        public ICommand RemoveProductCategoryCommand { get; }
        public ICommand SelectedProductCategoryChangedCommand { get; }

        #endregion

        #region Interface

        public SettingsProductCategoriesViewModel(IMessenger messenger, DbProductCategory dbProductCategory) : base(LocalizationStrings.SettingsProductCategories, typeof(SettingsProductCategories), messenger)
        {
            _dbProductCategory = dbProductCategory;

            AddProductCategoryCommand = new RelayCommand(AddProductCategory);
            EditProductCategoryCommand = new RelayCommand(EditProductCategory);
            RemoveProductCategoryCommand = new RelayCommand(RemoveProductCategory);
            SelectedProductCategoryChangedCommand = new RelayCommand(SelectedProductCategoryChanged);

            _messenger.Register<UpdateProductCategoryMsg>(UpdateProductCategory);
        }

        #endregion

        #region Private

        protected override async void PageLoaded(object obj)
        {
            base.PageLoaded(obj);

            await LoadCategories();
        }

        private async Task LoadCategories()
        {
            ObservableCollection<ProductCategoryModel> categories = null;

            await RunUnderBusyDialog(async () => categories = await _dbProductCategory.GetProductCategories());

            ProductCategories = categories;
        }

        private ProductCategoryModel GetProductCategoryByName(long id)
        {
            ProductCategoryModel res = null;
            
            RunUnderBusyDialog(() =>res = _dbProductCategory.GetProductCategoryById(id));
            
            return res;
        }

        private async void AddProductCategory(object obj)
        {
            var vm = new ProductCategoryEditViewModel(_messenger, ProductCategories);

            var dialog = new Controls.ContentDialog()
            {
                XamlRoot = _page.XamlRoot,
                Content = new ProductCategory.Components.ProductCategoryEdit()
                {
                    DataContext = vm
                },
                PrimaryButtonText = Localization.GetLocalizedString(LocalizationStrings.AddButton),
                CloseButtonText = Localization.GetLocalizedString(LocalizationStrings.CancelButton),
                DefaultButton = ContentDialogButton.Primary
            };
            dialog.PrimaryButtonCommandParameter = dialog;

            if (await dialog.ShowAsync() is ContentDialogResult.Primary)
            {
                _dbProductCategory.AddProductCategory(vm.Data);

                await LoadCategories();

                _messenger.Send(new ProductCategoriesUpdatedMsg());
            }
        }

        private async void EditProductCategory(object obj)
        {
            var productCatToEdit = obj.CheckType<ProductCategoryModel>();
            await EditProductCategoryCore(productCatToEdit);
        }

        private async Task EditProductCategoryCore(ProductCategoryModel productCatToEdit)
        {
            var vm = new ProductCategoryEditViewModel(_messenger, ProductCategories, productCatToEdit);

            var dialog = new Controls.ContentDialog()
            {
                XamlRoot = _page.XamlRoot,
                Content = new ProductCategory.Components.ProductCategoryEdit()
                {
                    DataContext = vm
                },
                PrimaryButtonText = Localization.GetLocalizedString(LocalizationStrings.EditButton),
                CloseButtonText = Localization.GetLocalizedString(LocalizationStrings.CancelButton),
                DefaultButton = ContentDialogButton.Primary
            };

            if (await dialog.ShowAsync() is ContentDialogResult.Primary)
            {
                _dbProductCategory.EditProductCategory(vm.Data);

                ProductCategories[ProductCategories.IndexOf(productCatToEdit)] = GetProductCategoryByName(productCatToEdit.ID);

                _messenger.Send(new ProductCategoriesUpdatedMsg());
                _messenger.Send(new ProductCategoryUpdatedMsg(productCatToEdit.ID));
            }
        }

        private async void RemoveProductCategory(object obj)
        {
            var productCatToRemove = obj.CheckType<ProductCategoryModel>();

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
                _dbProductCategory.RemoveProductCategory(productCatToRemove);

                ProductCategories.Remove(productCatToRemove);

                _messenger.Send(new ProductCategoriesUpdatedMsg());
                _messenger.Send(new ProductsUpdatedMsg()); 
            }
        }

        private void SelectedProductCategoryChanged(object obj)
        {
            if (obj is null)
                return;

            _navDetailFrame.Navigate(typeof(ProductCategoryDetail), null, new EntranceNavigationTransitionInfo());
            _messenger.Send(new SelectedProductCategoryChangedMsg(obj.CheckType<ProductCategoryModel>()));
        }

        private async void UpdateProductCategory(UpdateProductCategoryMsg obj) => await EditProductCategoryCore(obj.ProductCategoryData);

        #endregion
    }
}
