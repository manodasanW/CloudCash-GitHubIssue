using CloudCash.BL.DbAccess;
using CloudCash.BL.DTOs.TableCategories;
using CloudCash.BL.DTOs.TableInfo;
using CloudCash.Client.Modules.Settings.Messages;
using CloudCash.Client.Modules.Settings.ViewModels.Base;
using CloudCash.Client.Modules.Settings.Views;
using CloudCash.Client.Modules.TableInfo.Messages;
using CloudCash.Client.Modules.TableInfo.ViewModels;
using CloudCash.Client.Modules.TableInfo.Views;
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
    public class SettingsTableInfosViewModel : SettingsPartVMBase
    {
        private readonly DbTableInfo _dbTableInfo;
        private readonly DbTableCategory _dbTableCategory;
        private ObservableCollection<TableCategoryModel> _tableCategories = new();
        private ObservableCollection<TableInfoDetailModel> _tableInfos = new();

        #region Props

        public CollectionViewSource GroupedTableInfos { get; set; } = new();

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

        public ICommand AddTableInfoCommand { get; }
        public ICommand EditTableInfoCommand { get; }
        public ICommand RemoveTableInfoCommand { get; }
        public ICommand SelectedTableInfoChangedCommand { get; set; }

        #endregion

        #region Interface

        public SettingsTableInfosViewModel(IMessenger messenger, DbTableInfo dbTableInfo, DbTableCategory dbTableCategory) : base(LocalizationStrings.SettingsTableInfos, typeof(SettingsTableInfos), messenger)
        {
            _dbTableInfo = dbTableInfo;
            _dbTableCategory = dbTableCategory;

            AddTableInfoCommand = new RelayCommand(AddTableInfo);
            EditTableInfoCommand = new RelayCommand(EditTableInfo);
            RemoveTableInfoCommand = new RelayCommand(RemoveTableInfo);
            SelectedTableInfoChangedCommand = new RelayCommand(SelectedTableInfoChanged);

            _messenger.Register<TableCategoriesUpdatedMsg>(TableCategoriesUpdated);
            _messenger.Register<UpdateTableInfoMsg>(UpdateTableInfo);

            GroupedTableInfos.IsSourceGrouped = true;
        }

        #endregion

        #region Private

        protected override async void PageLoaded(object obj)
        {
            base.PageLoaded(obj);

            await LoadTableInfos();
            await LoadCategories();
        }

        private async Task LoadCategories()
        {
            await RunUnderBusyDialog(async () => _tableCategories = await _dbTableCategory.GetTableCategories());

            ErrorMessageVisible = _tableCategories.Count is 0;
        }

        private async Task LoadTableInfos()
        {
            await RunUnderBusyDialog(async () => _tableInfos = await _dbTableInfo.GetTableInfos());
            UpdateGroupedView();
        }

        private void UpdateGroupedView()
        {
            GroupedTableInfos.Source = _tableInfos.GroupBy(x => x.Category.Name);
            OnPropertyChanged(nameof(GroupedTableInfos));
        }

        private TableInfoDetailModel GetTableInfoById(long id)
        {
            TableInfoDetailModel res = null;

            RunUnderBusyDialog(() => res = _dbTableInfo.GetTableInfoByID(id));
            
            return res;
        }

        private async void AddTableInfo(object obj)
        {
            var vm = new TableInfoEditViewModel(_messenger, _tableInfos, _tableCategories);

            var dialog = new Controls.ContentDialog()
            {
                XamlRoot = _page.XamlRoot,
                Content = new TableInfo.Components.TableInfoEdit()
                {
                    DataContext = vm
                },
                PrimaryButtonText = Localization.GetLocalizedString(LocalizationStrings.AddButton),
                CloseButtonText = Localization.GetLocalizedString(LocalizationStrings.CancelButton),
                DefaultButton = ContentDialogButton.Primary
            };

            if (await dialog.ShowAsync() is ContentDialogResult.Primary)
            {
                _dbTableInfo.AddTableInfo(vm.Data);

                await LoadTableInfos();

                _messenger.Send(new TableInfosUpdatedMsg());
            }
        }

        private async void EditTableInfo(object obj)
        {
            var tableInfoToEdit = obj.CheckType<TableInfoDetailModel>();

            await EditTableInfoCore(tableInfoToEdit);
        }

        private async Task EditTableInfoCore(TableInfoDetailModel tableInfoToEdit)
        {
            var vm = new TableInfoEditViewModel(_messenger, _tableInfos, _tableCategories, tableInfoToEdit);

            var dialog = new Controls.ContentDialog()
            {
                XamlRoot = _page.XamlRoot,
                Content = new TableInfo.Components.TableInfoEdit()
                {
                    DataContext = vm
                },
                PrimaryButtonText = Localization.GetLocalizedString(LocalizationStrings.EditButton),
                CloseButtonText = Localization.GetLocalizedString(LocalizationStrings.CancelButton),
                DefaultButton = ContentDialogButton.Primary
            };

            if (await dialog.ShowAsync() is ContentDialogResult.Primary)
            {
                _dbTableInfo.EditTableInfo(vm.Data);

                _tableInfos[_tableInfos.IndexOf(tableInfoToEdit)] = GetTableInfoById(tableInfoToEdit.ID);
                UpdateGroupedView();

                _messenger.Send(new TableInfosUpdatedMsg());
                _messenger.Send(new TableInfoUpdatedMsg(tableInfoToEdit.ID));
            }
        }

        private async void RemoveTableInfo(object obj)
        {
            var tableInfoToRemove = obj.CheckType<TableInfoDetailModel>();

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
                _dbTableInfo.RemoveTableInfo(tableInfoToRemove);

                _tableInfos.Remove(tableInfoToRemove);
                UpdateGroupedView();

                _messenger.Send(new TableInfosUpdatedMsg());
            }
        }

        private void SelectedTableInfoChanged(object obj)
        {
            if (obj is null)
                return;

            _navDetailFrame.Navigate(typeof(TableInfoDetail), null, new EntranceNavigationTransitionInfo());
            _messenger.Send(new SelectedTableInfoChangedMsg(obj.CheckType<TableInfoDetailModel>()));
        }

        private async void TableCategoriesUpdated(TableCategoriesUpdatedMsg obj) => await LoadCategories();

        private async void UpdateTableInfo(UpdateTableInfoMsg obj) => await EditTableInfoCore(obj.TableInfoData);

        #endregion
    }
}
