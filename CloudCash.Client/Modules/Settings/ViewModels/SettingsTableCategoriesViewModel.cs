using CloudCash.BL.DbAccess;
using CloudCash.BL.DTOs.TableCategories;
using CloudCash.Client.Modules.Settings.Messages;
using CloudCash.Client.Modules.Settings.ViewModels.Base;
using CloudCash.Client.Modules.Settings.Views;
using CloudCash.Client.Modules.TableCategory.Messages;
using CloudCash.Client.Modules.TableCategory.ViewModels;
using CloudCash.Client.Modules.TableCategory.Views;
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
    public class SettingsTableCategoriesViewModel : SettingsPartVMBase
    {
        private readonly DbTableCategory _dbTableCategory;

        #region Props

        private ObservableCollection<TableCategoryModel> _tableCategories = new();
        public ObservableCollection<TableCategoryModel> TableCategories
        {
            get => _tableCategories;
            set
            {
                _tableCategories = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand AddTableCategoryCommand { get; }
        public ICommand EditTableCategoryCommand { get; }
        public ICommand RemoveTableCategoryCommand { get; }
        public ICommand SelectedTableCategoryChangedCommand { get; set; }

        #endregion

        #region Interface

        public SettingsTableCategoriesViewModel(IMessenger messenger, DbTableCategory dbTableCategory) : base(LocalizationStrings.SettingsTableCategories, typeof(SettingsTableCategories), messenger)
        {
            _dbTableCategory = dbTableCategory;

            AddTableCategoryCommand = new RelayCommand(AddTableCategory);
            EditTableCategoryCommand = new RelayCommand(EditTableCategory);
            RemoveTableCategoryCommand = new RelayCommand(RemoveTableCategory);
            SelectedTableCategoryChangedCommand = new RelayCommand(SelectedTableCategoryChanged);

            _messenger.Register<UpdateTableCategoryMsg>(UpdateTableCategory);
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
            ObservableCollection<TableCategoryModel> categories = null;
            
            await RunUnderBusyDialog(async () => categories= await _dbTableCategory.GetTableCategories());

            TableCategories = categories;
        }

        private TableCategoryModel GetTableCategoryByName(long id)
        {
            TableCategoryModel res = null;
            
            RunUnderBusyDialog(() => res = _dbTableCategory.GetTableCategoryById(id));
            
            return res;
        }

        private async void AddTableCategory(object obj)
        {
            var vm = new TableCategoryEditViewModel(_messenger, TableCategories);            

            var dialog = new Controls.ContentDialog()
            {
                XamlRoot = _page.XamlRoot,
                Content = new TableCategory.Components.TableCategoryEdit()
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
                _dbTableCategory.AddTableCategory(vm.Data);

                await LoadCategories();

                _messenger.Send(new TableCategoriesUpdatedMsg());
            }
        }

        private async void EditTableCategory(object obj)
        {
            var tableCatToEdit = obj.CheckType<TableCategoryModel>();
            await EditTableCategoryCore(tableCatToEdit);
        }

        private async Task EditTableCategoryCore(TableCategoryModel tableCatToEdit)
        {
            var vm = new TableCategoryEditViewModel(_messenger, TableCategories, tableCatToEdit);

            var dialog = new Controls.ContentDialog()
            {
                XamlRoot = _page.XamlRoot,
                Content = new TableCategory.Components.TableCategoryEdit()
                {
                    DataContext = vm
                },
                PrimaryButtonText = Localization.GetLocalizedString(LocalizationStrings.EditButton),
                CloseButtonText = Localization.GetLocalizedString(LocalizationStrings.CancelButton),
                DefaultButton = ContentDialogButton.Primary
            };

            if (await dialog.ShowAsync() is ContentDialogResult.Primary)
            {
                _dbTableCategory.EditTableCategory(vm.Data);

                TableCategories[TableCategories.IndexOf(tableCatToEdit)] = GetTableCategoryByName(tableCatToEdit.ID);

                _messenger.Send(new TableCategoriesUpdatedMsg());
                _messenger.Send(new TableCategoryUpdatedMsg(tableCatToEdit.ID));
            }
        }

        private async void RemoveTableCategory(object obj)
        {
            var tableCatToRemove = obj.CheckType<TableCategoryModel>();

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
                _dbTableCategory.RemoveTableCategory(tableCatToRemove);

                TableCategories.Remove(tableCatToRemove);

                _messenger.Send(new TableCategoriesUpdatedMsg());
                _messenger.Send(new TableInfosUpdatedMsg());
            }
        }

        private void SelectedTableCategoryChanged(object obj)
        {
            if (obj is null)
                return;

            _navDetailFrame.Navigate(typeof(TableCategoryDetail), null, new EntranceNavigationTransitionInfo());
            _messenger.Send(new SelectedTableCategoryChangedMsg(obj.CheckType<TableCategoryModel>()));
        }

        private async void UpdateTableCategory(UpdateTableCategoryMsg obj) => await EditTableCategoryCore(obj.TableCategoryData);

        #endregion
    }
}
