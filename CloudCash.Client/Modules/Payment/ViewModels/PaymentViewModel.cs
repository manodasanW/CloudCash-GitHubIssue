using CloudCash.BL.DbAccess;
using CloudCash.BL.DTOs.Payments;
using CloudCash.BL.DTOs.Sells;
using CloudCash.BL.DTOs.Tables;
using CloudCash.Client.Modules.MainPage.Messages;
using CloudCash.Client.Modules.Payment.Messages;
using CloudCash.Client.Modules.ProductSell.Classes;
using CloudCash.Client.Modules.Table.Messages;
using CloudCash.Common.Enums;
using CloudCash.Common.Functions;
using CloudCash.Common.MVVM;
using CloudCash.Interface.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CloudCash.Client.Modules.Payment.ViewModels
{
    public class PaymentViewModel : ViewModelBase
    {

        private readonly DbCard _dbCard;
        private readonly DbCustomer _dbCustomer;
        private readonly DbPayment _dbPayment;
        private readonly DbProduct _dbProduct;
        private readonly DbSell _dbSell;
        private readonly DbTable _dbTable;
        private readonly DbUser _dbUser;

        #region Props

        private ObservableCollection<ProductSellsModel> _sellsOnTable = new();
        public ObservableCollection<ProductSellsModel> SellsOnTable
        {
            get => _sellsOnTable;
            set
            {
                _sellsOnTable = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ProductSellsModel> _sellsToPay = new();
        public ObservableCollection<ProductSellsModel> SellsToPay
        {
            get => _sellsToPay;
            set
            {
                _sellsToPay = value;
                OnPropertyChanged();
            }
        }

        private TableDetailModel _tableInfo;
        public TableDetailModel TableInfo
        {
            get => _tableInfo;
            set
            {
                _tableInfo = value;
                OnPropertyChanged();
            }
        }

        private PaymentDetailModel _paymentInfo = new();
        public PaymentDetailModel PaymentInfo
        {
            get => _paymentInfo;
            set
            {
                _paymentInfo = value;
                OnPropertyChanged();
            }
        }

        private List<bool> _selectedPaymentType = GetListForPaymentSelection();
        public List<bool> SelectedPaymentType
        {
            get => _selectedPaymentType;
            set
            {
                _selectedPaymentType = value;
                OnPropertyChanged();
                UpdatePaymentType();
            }
        }

        private bool _showSplitTable;
        public bool ShowSplitTable
        {
            get => _showSplitTable;
            set
            {
                _showSplitTable = value;
                OnPropertyChanged();
            }
        }

        public bool IsPayEnabled => PriceToPay is not 0;

        public uint PriceToPay => (uint)(ShowSplitTable ? SellsToPay : SellsOnTable).Sum(x => x.SpendedCash);

        public byte Discount => 0;

        #endregion

        #region Commands

        public ICommand SelectUserCommand { get; }
        public ICommand PayCommand { get; }
        public ICommand GoBackCommand { get; }
        public ICommand AddSellToPayCommand { get; }
        public ICommand AddProductToPayCommand { get; }
        public ICommand RemoveSellFromPayCommand { get; }
        public ICommand RemoveProductToPayCommand { get; }
        public ICommand SwitchToSplitTableCommand { get; }

        #endregion

        #region Interface

        public PaymentViewModel(IMessenger messenger, DbPayment dbPayment, DbSell dbSell, DbCustomer dbCustomer, DbTable dbTable, DbUser dbUser, DbCard dbCard, DbProduct dbProduct) : base(messenger)
        {
            _dbPayment = dbPayment;
            _dbSell = dbSell;
            _dbCustomer = dbCustomer;
            _dbTable = dbTable;
            _dbUser = dbUser;
            _dbCard = dbCard;
            _dbProduct = dbProduct;

            AddSellToPayCommand = new RelayCommand(AddSellToPay);
            AddProductToPayCommand = new RelayCommand(AddProductToPay);
            RemoveSellFromPayCommand = new RelayCommand(RemoveSellFromPay);
            RemoveProductToPayCommand = new RelayCommand(RemoveProductToPay);
            GoBackCommand = new RelayCommand(GoBack);
            PayCommand = new RelayCommand(Pay);
            SwitchToSplitTableCommand = new RelayCommand(SwitchToSplitTable);

            _messenger.Register<TableToPaymentMsg>(LoadTableForPayment);
        }

        #endregion

        #region Private

        private void UpdatePaymentType() => PaymentInfo.PaymentType = (PaymentType)SelectedPaymentType.IndexOf(true);

        private void RemoveProductToPay(object obj)
        {
            var productSells = obj.CheckType<ProductSellsModel>();

            SellsOnTable.Add(productSells);
            SellsToPay.Remove(productSells);

            UpdatePaymentView();
        }

        private void RemoveSellFromPay(object obj)
        {
            var productSells = obj.CheckType<ProductSellsModel>();

            SellsOnTable.AddSellToProductSell(productSells.RemoveSell());

            if (productSells.SellCount is 0)
                SellsToPay.Remove(productSells);

            UpdatePaymentView();
        }

        private void AddProductToPay(object obj)
        {
            var productSells = obj.CheckType<ProductSellsModel>();

            SellsToPay.Add(productSells);
            SellsOnTable.Remove(productSells);

            UpdatePaymentView();
        }

        private void AddSellToPay(object obj)
        {
            var productSells = obj.CheckType<ProductSellsModel>();

            SellsToPay.AddSellToProductSell(productSells.RemoveSell());

            if (productSells.SellCount is 0)
                SellsOnTable.Remove(productSells);

            UpdatePaymentView();
        }

        private void UpdatePaymentView()
        {
            OnPropertyChanged(nameof(PriceToPay));
            OnPropertyChanged(nameof(IsPayEnabled));

            PaymentInfo.Price = (uint)(PriceToPay * (1 - (Discount / 100)));
            PaymentInfo.IsPartial = (SellsOnTable.Count is not 0 && ShowSplitTable) || (TableInfo.Payments.Count is not 0 && SellsOnTable.Count is 0);

            OnPropertyChanged(nameof(PaymentInfo));
            OnPropertyChanged(nameof(SellsToPay));
        }

        private void SwitchToSplitTable(object obj)
        {
            ShowSplitTable = true;
            UpdatePaymentView();
        }

        private async void LoadTableForPayment(TableToPaymentMsg obj)
        {
            TableInfo = _dbTable.GetTableById(obj.TableId);

            RestartPayment();
            await LoadSells(obj);
            UpdatePaymentView();
        }

        private void RestartPayment()
        {
            PaymentInfo = new();
            SellsToPay.Clear();
        }

        private async Task LoadSells(TableToPaymentMsg obj)
        {
            List<ProductSellsModel> productSells = null;

            await RunUnderBusyDialog(async () =>
            {
                List<SellDetailModel> sells = (await _dbSell.GetUnpaidSellsByTableId(obj.TableId)).ToList();
                productSells = ProductSellsSupport.ConvertSellsToProductSells(sells);
            });

            SellsOnTable = new(productSells);
        }

        private void GoBack(object obj) => _messenger.Send(new NavigateBackMsg());

        private void Pay(object obj)
        {
            PreparePaymentData();

            _dbPayment.AddPayment(PaymentInfo);

            var paymentCompletedMsg = new PaymentCompletedMsg(TableInfo.TableInfo.ID, PaymentInfo.Price);

            if (!PaymentInfo.IsPartial || (SellsOnTable.Count is 0 && ShowSplitTable))
            {
                _dbTable.CloseTable(TableInfo);
                _messenger.Send(new NavigateToMsg(typeof(TablesView.Views.TablesView)));

                paymentCompletedMsg.TableClosed = true;
                ShowSplitTable = false;
            }

            RestartPayment();
            UpdatePaymentView();

            _messenger.Send(paymentCompletedMsg);
            TableInfo = _dbTable.GetTableById(TableInfo.ID);
        }

        private void PreparePaymentData()
        {
            PaymentInfo.Sells = GetSellsToPay();
            PaymentInfo.Table = TableInfo;
            PaymentInfo.DateTime = DateTime.Now;
            UpdatePaymentType();
        }

        private ICollection<SellDetailModel> GetSellsToPay()
        {
            var sells = new List<SellDetailModel>();
            var sellsCollection = SellsOnTable;

            if (ShowSplitTable)
                sellsCollection = SellsToPay;

            foreach (var productSell in sellsCollection)
                sells.AddRange(productSell.Sells);

            return sells;
        }

        private static List<bool> GetListForPaymentSelection()
        {
            var boolListLength = Enum.GetValues(typeof(PaymentType)).Length;
            var boolList = new List<bool>();

            for (int i = 0; i < boolListLength; i++)
            {
                boolList.Add(false);
            }

            boolList[0] = true;

            return boolList;
        }

        #endregion
    }
}