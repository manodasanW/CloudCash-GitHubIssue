using CloudCash.BL.DbAccess;
using CloudCash.BL.DTOs.Sells;
using CloudCash.BL.DTOs.TableInfo;
using CloudCash.BL.DTOs.Tables;
using CloudCash.Client.Modules.MainPage.Messages;
using CloudCash.Client.Modules.Payment.Messages;
using CloudCash.Client.Modules.ProductSelector.Messages;
using CloudCash.Client.Modules.ProductSell.Classes;
using CloudCash.Client.Modules.Sell.Controls;
using CloudCash.Client.Modules.Table.Messages;
using CloudCash.Client.Modules.TablesView.Messages;
using CloudCash.Common.Enums;
using CloudCash.Common.Functions;
using CloudCash.Common.MVVM;
using CloudCash.Interface.Common;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CloudCash.Client.Modules.Table.ViewModels
{
    public class TableViewModel : ViewModelBase
    {
        private readonly DbCard _dbCard;
        private readonly DbPayment _dbPayment;
        private readonly DbProduct _dbProduct;
        private readonly DbProductCategory _dbProductCategory;
        private readonly DbReservation _dbReservations;
        private readonly DbCustomer _dbCustomer;
        private readonly DbSell _dbSell;
        private readonly DbTable _dbTable;
        private readonly DbTableCategory _dbTableCategory;
        private readonly DbTableInfo _dbTableInfo;
        private readonly DbUser _dbUser;

        private List<SellDetailModel> _newItemsToSell = new();

        #region Props

        private TableInfoDetailModel _tableInfo;
        public TableInfoDetailModel TableInfo
        {
            get => _tableInfo;
            set
            {
                _tableInfo = value;
                OnPropertyChanged();
            }
        }

        private TableDetailModel _table;
        public TableDetailModel Table
        {
            get => _table;
            set
            {
                _table = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ProductSellsModel> _soldProducts = new();
        public ObservableCollection<ProductSellsModel> SoldProducts
        {
            get => _soldProducts;
            set
            {
                _soldProducts = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SpendedCash));
            }
        }

        public bool IsPayEnabled => Table is not null && Table.ID is not 0;

        public uint SpendedCash => (uint)SoldProducts.Sum(x => x.SpendedCash);

        #endregion

        #region Commands

        public ICommand IncrementProductCommand { get; }
        public ICommand DecrementProductCommand { get; }
        public ICommand RemoveProductCommand { get; }
        public ICommand SplitSellCommand { get; }
        public ICommand EditProductCommand { get; }
        public ICommand MoveTableToAnotherTableCommand { get; }
        public ICommand GoToPaymentCommand { get; }
        public ICommand GoBackCommand { get; }

        #endregion

        #region Interface

        public TableViewModel(IMessenger messenger, DbTable dbTable, DbTableCategory dbTableCategory, DbTableInfo dbTableInfo,
            DbSell dbSell, DbPayment dbPayment, DbCustomer dbCustomer, DbProduct dbProduct, DbProductCategory dbProductCategory,
            DbReservation dbReservations, DbCard dbCard, DbUser dbUser) : base(messenger)
        {
            _dbTable = dbTable;
            _dbTableCategory = dbTableCategory;
            _dbTableInfo = dbTableInfo;
            _dbSell = dbSell;
            _dbPayment = dbPayment;
            _dbCustomer = dbCustomer;
            _dbProduct = dbProduct;
            _dbProductCategory = dbProductCategory;
            _dbReservations = dbReservations;
            _dbCard = dbCard;
            _dbUser = dbUser;

            IncrementProductCommand = new RelayCommand(IncrementProduct);
            DecrementProductCommand = new RelayCommand(DecrementProduct);
            SplitSellCommand = new RelayCommand(SplitSell);
            EditProductCommand = new RelayCommand(EditProduct);
            RemoveProductCommand = new RelayCommand(RemoveProduct);
            GoBackCommand = new RelayCommand(GoBack);
            GoToPaymentCommand = new RelayCommand(GoToPayment);

            _messenger.Register<AddProductToTableMsg>(ProductAddedToTable);
            _messenger.Register<SelectedTableInfoChangedMsg>(SelctedTableChanged);
            _messenger.Register<PaymentCompletedMsg>(PaymentCompleted);
        }

        #endregion

        #region Private

        private async void EditProduct(object obj)
        {
            var productSells = obj.CheckType<ProductSellsModel>();
            var sells = productSells.Sells;
            var referenceSell = sells.First();
            var sellEditResult = await GeDiscount(referenceSell.Discount, sells.Count);

            if (sellEditResult.Count is -1)
                return;

            var callRemoveProduct = false;
            List<SellDetailModel> reloadedSells = new();
            var reloadSells = true;

            await RunUnderBusyDialog(async () =>
            {
                await Task.Run(async () =>
                {
                    var newCount = sellEditResult.Count - sells.Count;
                    var discount = sellEditResult.Discount;

                    if (sellEditResult.Count is 0)
                        callRemoveProduct = true;
                    else if (newCount > 0)
                    {
                        AddSellRange(referenceSell, newCount, discount);
                        reloadedSells = (await _dbSell.GetSellsReload(referenceSell.Product.ID, referenceSell.Table.ID, discount)).ToList();
                    }
                    else if (newCount < 0)
                    {
                        var sellsToDelete = sells.Take(-newCount).ToList();
                        _dbSell.RemoveSellRange(sellsToDelete);

                        foreach (var sell in sellsToDelete)
                            sells.Remove(sell);
                    }

                    if (discount != referenceSell.Discount)
                        _dbSell.SetDiscountRange(sells.Select(x => x.CopyToDetail()).ToList(), discount);

                    if (discount != referenceSell.Discount || newCount != 0)
                        reloadedSells = (await _dbSell.GetSellsReload(referenceSell.Product.ID, referenceSell.Table.ID, discount)).ToList();
                    else
                        reloadSells = false;
                });
            });

            if (callRemoveProduct)
                RemoveProduct(obj);

            if (reloadSells)
            {
                productSells.SetSells(reloadedSells);
                OnPropertyChanged(nameof(SoldProducts));
            }
        }

        // todo
        private async Task SetDiscountAndUpdateSells(ProductSellsModel productSells, List<SellDetailModel> sells, SellDetailModel referenceSell, (byte Discount, int Count) sellEditResult)
        {
            List<SellDetailModel> reloadedSells = new();

            await Task.Run(async () =>
            {
                var newCount = sellEditResult.Count - sells.Count;
                var discount = sellEditResult.Discount;

                if (newCount > 0)
                    AddSellRange(referenceSell, newCount, discount);
                else if (newCount < 0)
                    _dbSell.RemoveSellRange(sells.Take(-newCount).ToList());

                _dbSell.SetDiscountRange(sells.Select(x => x.CopyToDetail()).ToList(), discount);

                reloadedSells = (await _dbSell.GetSellsReload(referenceSell.Product.ID, referenceSell.Table.ID, discount)).ToList();
            });

            productSells.SetSells(reloadedSells);
        }

        private async Task<(byte Discount, int Count)> GeDiscount(byte discount, int count)
        {
            var tableProductSellEdit = new TableProductSellEdit(discount, count);

            ContentDialog dialog = new()
            {
                XamlRoot = _page.XamlRoot,
                Content = tableProductSellEdit,
                PrimaryButtonText = Localization.GetLocalizedString(LocalizationStrings.OkButton),
                CloseButtonText = Localization.GetLocalizedString(LocalizationStrings.CancelButton),
                DefaultButton = ContentDialogButton.Primary
            };

            if (await dialog.ShowAsync() is ContentDialogResult.Primary)
                return tableProductSellEdit.GetInsertedValues();

            return (0, -1);
        }

        private async void SplitSell(object obj)
        {
            var productSells = obj.CheckType<ProductSellsModel>();
            var sells = productSells.Sells;
            var referenceSell = sells.First();
            var splitToX = await GetHowMuchSplit();

            if (splitToX == -1)
                return;

            var discount = (byte)(100 - (100 - referenceSell.Discount) / splitToX);
            List<SellDetailModel> reloadedSells = new();

            await RunUnderBusyDialog(async () =>
            {
                await Task.Run(async () =>
                {
                    _dbSell.SetDiscountRange(sells.Select(x => x.CopyToDetail()).ToList(), discount);

                    AddSellRange(referenceSell, sells.Count * (splitToX - 1), discount);

                    reloadedSells = (await _dbSell.GetSellsReload(referenceSell.Product.ID, referenceSell.Table.ID, discount)).ToList();
                });
            });

            productSells.SetSells(reloadedSells);
            OnPropertyChanged(nameof(SoldProducts));
        }

        private List<SellDetailModel> AddSellRange(SellDetailModel referenceSell, int count, byte discount)
        {
            var newSells = new List<SellDetailModel>();

            for (int i = 0; i < count; i++)
            {
                newSells.Add(new()
                {
                    Discount = discount,
                    Product = referenceSell.Product,
                    Table = referenceSell.Table
                });
            }

            _dbSell.AddSellRange(newSells);

            return newSells;
        }

        private async Task<int> GetHowMuchSplit()
        {
            var splitControl = new TableSellSplit();

            ContentDialog dialog = new()
            {
                XamlRoot = _page.XamlRoot,
                Content = splitControl,
                PrimaryButtonText = Localization.GetLocalizedString(LocalizationStrings.OkButton),
                CloseButtonText = Localization.GetLocalizedString(LocalizationStrings.CancelButton),
                DefaultButton = ContentDialogButton.Primary
            };

            if (await dialog.ShowAsync() is ContentDialogResult.Primary)
                return splitControl.GetSellSplitCount();

            return -1;
        }

        private async void RemoveProduct(object obj)
        {
            var referenceSell = obj.CheckType<ProductSellsModel>().Sells.First();

            await Task.Run(() =>
            {
                _dbSell.RemoveSellsFromTable(referenceSell);
            });

            SoldProducts.RemoveAllSellsFromProductSell(referenceSell);
            OnPropertyChanged(nameof(SoldProducts));
        }

        private void DecrementProduct(object obj)
        {
            var sellToDelete = obj.CheckType<ProductSellsModel>().Sells.First();

            _dbSell.RemoveSellById(sellToDelete.ID);

            SoldProducts.RemoveSellFromProductSell(sellToDelete);
            OnPropertyChanged(nameof(SoldProducts));
        }

        private void IncrementProduct(object obj)
        {
            var sellIncrement = obj.CheckType<ProductSellsModel>().Sells.First().CopyToDetail();

            _dbSell.AddSell(sellIncrement);

            UpdateSells(sellIncrement);
        }

        private void PaymentCompleted(PaymentCompletedMsg obj)
        {
            if (obj.TableInfoId == Table.TableInfo.ID)
                GetTableSelledProducts();
        }

        private void SelctedTableChanged(SelectedTableInfoChangedMsg obj)
        {
            TableInfoDetailModel tableInfo = null;
            TableDetailModel table = null;

            RunUnderBusyDialog(() =>
            {
                tableInfo = _dbTableInfo.GetTableInfoByID(obj.TableInfoId);

                if (obj.IsOpen)
                    table = _dbTable.GetTableById(obj.TableId);
            });

            TableInfo = tableInfo;
            Table = table;

            OnPropertyChanged(nameof(IsPayEnabled));
            GetTableSelledProducts();
        }

        private void GoToPayment(object obj)
        {
            _messenger.Send(new NavigateToMsg(typeof(Payment.Views.Payment)));
            _messenger.Send(new TableToPaymentMsg(Table.ID));
        }

        private async void GetTableSelledProducts()
        {
            if (Table is null)
                return;

            List<ProductSellsModel> productSells = null;

            await RunUnderBusyDialog(async () =>
            {
                List<SellDetailModel> sells = (await _dbSell.GetUnpaidSellsByTableId(Table.ID)).ToList();
                productSells = ProductSellsSupport.ConvertSellsToProductSells(sells);
            });

            SoldProducts = new(productSells);
        }

        private void ProductAddedToTable(AddProductToTableMsg obj)
        {
            CheckIfOpenTable();

            AddProductToTable(obj);
        }

        private void CheckIfOpenTable()
        {
            if (Table is not null)
                return;

            var table = new TableDetailModel
            {
                TableInfo = TableInfo
            };

            _dbTable.AddTable(table);
            Table = table;

            _messenger.Send(new TableOpenedMsg(table.ID));
            OnPropertyChanged(nameof(IsPayEnabled));
        }

        private void AddProductToTable(AddProductToTableMsg obj)
        {
            SellDetailModel sellDetail = CreateSell(obj);

            _newItemsToSell.Add(new()
            {
                Product = obj.NewItemToSell,
                Table = Table
            });

            UpdateSells(sellDetail);
        }

        private void UpdateSells(SellDetailModel sellDetail)
        {
            SoldProducts.AddSellToProductSell(sellDetail);
            OnPropertyChanged(nameof(SoldProducts));
        }

        private SellDetailModel CreateSell(AddProductToTableMsg obj)
        {
            var sellDetail = new SellDetailModel
            {
                Discount = 0,
                Product = obj.NewItemToSell,
                Table = Table
            };

            _dbSell.AddSell(sellDetail);
            return sellDetail;
        }

        private void GoBack(object obj)
        {
            SendSellsToPrinter();
            UpdateTableLastSell();
            _messenger.Send(new NavigateBackMsg());
        }

        private void SendSellsToPrinter()
        {
            // todo finish
        }

        private void UpdateTableLastSell()
        {
            //_messenger.Send(new );
        }

        #endregion
    }
}
