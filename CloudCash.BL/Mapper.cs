using CloudCash.BL.DTOs.Cards;
using CloudCash.BL.DTOs.Customers;
using CloudCash.BL.DTOs.ExpenseIncomes;
using CloudCash.BL.DTOs.Payments;
using CloudCash.BL.DTOs.ProductCategories;
using CloudCash.BL.DTOs.Products;
using CloudCash.BL.DTOs.Reservations;
using CloudCash.BL.DTOs.Sells;
using CloudCash.BL.DTOs.Shifts;
using CloudCash.BL.DTOs.TableCategories;
using CloudCash.BL.DTOs.TableInfo;
using CloudCash.BL.DTOs.Tables;
using CloudCash.BL.DTOs.UserLog;
using CloudCash.BL.DTOs.Users;
using CloudCash.DAL.Entities;
using System.Collections.ObjectModel;
using System.Linq;

namespace CloudCash.BL
{
    public static class Mapper
    {
        public static CustomerDetailModel MapEntityToDetailModel(Customer customer) => new()
        {
            ID = customer.ID,
            Email = customer.Email,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            BonusPoints = customer.BonusPoints,
            Card = MapEntityToListModel(customer.Card)
        };

        public static CustomerListModel MapEntityToListModel(Customer customer) => customer is null ? null : new()
        {
            ID = customer.ID,
            FirstName = customer.FirstName,
            LastName = customer.LastName
        };

        public static PaymentDetailModel MapEntityToDetailModel(Payment payment) => payment is null ? null : new()
        {
            ID = payment.ID,
            Customer = MapEntityToListModel(payment.Customer),
            DateTime = payment.DateTime,
            IsPartial = payment.IsPartial,
            Price = payment.Price,
            Sells = new ObservableCollection<SellDetailModel>(payment.Sells.Select(MapEntityToListModel)),
            Table = MapEntityToListModel(payment.Table),
            PaymentType = payment.PaymentType,
            Note = payment.Note,
            Discount = payment.Discount
        };

        public static PaymentListModel MapEntityToListModel(Payment payment) => payment is null ? null : new()
        {
            ID = payment.ID,
            DateTime = payment.DateTime,
            Table = MapEntityToListModel(payment.Table),
            IsPartial = payment.IsPartial,
            Price = payment.Price
        };

        public static ProductCategoryModel MapEntityToDetailModel(ProductCategory productCategory) => new()
        {
            ID = productCategory.ID,
            Name = productCategory.Name,
            PrintSeparately = productCategory.PrintSeparately
        };

        public static ProductCategoryModel MapEntityToListModel(ProductCategory productCategory) => MapEntityToDetailModel(productCategory);

        public static ProductDetailModel MapEntityToDetailModel(Product product) => new()
        {
            ID = product.ID,
            Name = product.Name,
            VatLevel = product.VatLevel,
            Price = product.Price,
            Category = MapEntityToListModel(product.Category)
        };

        public static ProductListModel MapEntityToListModel(Product product) => new()
        {
            ID = product.ID,
            Name = product.Name,
            VatLevel = product.VatLevel,
            Price = product.Price,
            Category = MapEntityToListModel(product.Category)
        };

        public static SellDetailModel MapEntityToDetailModel(Sell sell) => new()
        {
            ID = sell.ID,
            DateTime = sell.DateTime,
            Discount = sell.Discount,
            Product = MapEntityToListModel(sell.Product),
            Table = MapEntityToListModel(sell.Table),
            Payment = MapEntityToListModel(sell.Payment)
        };

        public static SellDetailModel MapEntityToListModel(Sell sell) => MapEntityToDetailModel(sell);

        public static ShiftDetailModel MapEntityToDetailModel(Shift shift) => new()
        {
            ID = shift.ID,
            CashValue = shift.CashValue,
            DateTime = shift.DateTime,
            ShiftRecordType = shift.ShiftRecordType,
            User = MapEntityToListModel(shift.User ?? new())
        };

        public static ShiftListModel MapEntityToListModel(Shift shift) => new()
        {
            ID = shift.ID,
            DateTime = shift.DateTime,
            ShiftRecordType = shift.ShiftRecordType
        };

        public static TableCategoryModel MapEntityToDetailModel(TableCategory tableCategory) => new()
        {
            ID = tableCategory.ID,
            Name = tableCategory.Name
        };

        public static TableCategoryModel MapEntityToListModel(TableCategory tableCategory) => MapEntityToDetailModel(tableCategory);

        public static TableInfoDetailModel MapEntityToDetailModel(TableInfo tableInfo) => new()
        {
            ID = tableInfo.ID,
            Name = tableInfo.Name,
            PositionX = tableInfo.PositionX,
            PositionY = tableInfo.PositionY,
            Size = tableInfo.Size,
            Category = MapEntityToListModel(tableInfo.Category)
        };

        public static TableInfoDetailModel MapEntityToListModel(TableInfo tableInfo) => MapEntityToDetailModel(tableInfo);

        public static TableDetailModel MapEntityToDetailModel(Table table) => new()
        {
            ID = table.ID,
            EndDateTime = table.EndDateTime,
            Payments = new ObservableCollection<PaymentDetailModel>(table.Payments.Select(MapEntityToDetailModel)),
            Sells = new ObservableCollection<SellDetailModel>(table.Sells.Select(MapEntityToDetailModel)),
            StartDateTime = table.StartDateTime,
            TableInfo = MapEntityToListModel(table.TableInfo)
        };

        public static TableListModel MapEntityToListModel(Table table) => new()
        {
            ID = table.ID,
            StartDateTime = table.StartDateTime,
            TableInfo = MapEntityToListModel(table.TableInfo)
        };

        public static UserDetailModel MapEntityToDetailModel(User user) => new()
        {
            ID = user.ID,
            FirstName = user.FirstName,
            Hash = user.Hash,
            LastName = user.LastName,
            NickName = user.NickName,
            Rights = user.Rights,
            Salt = user.Salt
        };

        public static UserListModel MapEntityToListModel(User user) => new()
        {
            ID = user.ID,
            LastName = user.LastName,
            FirstName = user.FirstName,
            NickName = user.NickName
        };

        public static CardDetailModel MapEntityToDetailModel(Card card) => new()
        {
            ID = card.ID,
            Number = card.Number
        };

        public static CardListModel MapEntityToListModel(Card card) => MapEntityToDetailModel(card);

        public static ReservationDetailModel MapEntityToDetailModel(Reservation reservation) => new()
        {
            ID = reservation.ID,
            Email = reservation.Email,
            StartTime = reservation.StartTime,
            EndTime = reservation.EndTime,
            PeopleCount = reservation.PeopleCount,
            SelectedTable = MapEntityToListModel(reservation.SelectedTable)
        };

        public static ReservationListModel MapEntityToListModel(Reservation reservation) => new()
        {
            ID = reservation.ID,
            EndTime = reservation.EndTime,
            StartTime = reservation.StartTime,
            Name = reservation.Name
        };

        public static ExpenseIncomeDetailModel MapEntityToDetailModel(ExpenseIncome expenseIncome) => new()
        {
            ID = expenseIncome.ID,
            DateTime = expenseIncome.DateTime,
            EIType = expenseIncome.EIType,
            Price = expenseIncome.Price,
            User = MapEntityToListModel(expenseIncome.User)
        };

        public static ExpenseIncomeListModel MapEntityToListModel(ExpenseIncome expenseIncome) => new()
        {
            ID = expenseIncome.ID,
            Price = expenseIncome.Price,
            EIType = expenseIncome.EIType,
            DateTime = expenseIncome.DateTime
        };

        public static UserLogDetailModel MapEntityToDetailModel(UserLog userLog) => new()
        {
            ID = userLog.ID,
            DateTime = userLog.DateTime,
            User = MapEntityToListModel(userLog.User),
            LogType = userLog.LogType
        };

        public static UserLogDetailModel MapEntityToListModel(UserLog userLog) => MapEntityToDetailModel(userLog);

        public static Customer MapDetailModelToEntity(CustomerDetailModel customerDetailModel) => customerDetailModel is null ? null : new()
        {
            ID = customerDetailModel.ID,
            Email = customerDetailModel.Email,
            FirstName = customerDetailModel.FirstName,
            LastName = customerDetailModel.LastName,
            BonusPoints = customerDetailModel.BonusPoints,
            Card = MapListModelToEntity(customerDetailModel.Card)
        };

        public static Customer MapListModelToEntity(CustomerListModel customerListModel) => customerListModel is null ? null : new()
        {
            ID = customerListModel.ID,
            FirstName = customerListModel.FirstName,
            LastName = customerListModel.LastName
        };

        public static Payment MapDetailModelToEntity(PaymentDetailModel paymentDetailModel) => paymentDetailModel is null ? null : new()
        {
            ID = paymentDetailModel.ID,
            Customer = MapListModelToEntity(paymentDetailModel.Customer),
            DateTime = paymentDetailModel.DateTime,
            IsPartial = paymentDetailModel.IsPartial,
            Price = paymentDetailModel.Price,
            Sells = new ObservableCollection<Sell>(paymentDetailModel.Sells.Select(MapListModelToEntity)),
            Table = MapListModelToEntity(paymentDetailModel.Table),
            Discount = paymentDetailModel.Discount,
            Note = paymentDetailModel.Note,
            PaymentType = paymentDetailModel.PaymentType
        };

        public static Payment MapListModelToEntity(PaymentListModel paymentListModel) => paymentListModel is null ? null : new()
        {
            ID = paymentListModel.ID,
            IsPartial = paymentListModel.IsPartial,
            Price = paymentListModel.Price,
            Table = MapListModelToEntity(paymentListModel.Table),
            DateTime = paymentListModel.DateTime
        };

        public static ProductCategory MapDetailModelToEntity(ProductCategoryModel productCategoryModel) => new()
        {
            ID = productCategoryModel.ID,
            Name = productCategoryModel.Name,
            PrintSeparately = productCategoryModel.PrintSeparately
        };

        public static ProductCategory MapListModelToEntity(ProductCategoryModel productCategoryModel) => MapDetailModelToEntity(productCategoryModel);

        public static Product MapDetailModelToEntity(ProductDetailModel productDetailModel) => new()
        {
            ID = productDetailModel.ID,
            Name = productDetailModel.Name,
            Category = MapListModelToEntity(productDetailModel.Category),
            Price = productDetailModel.Price,
            VatLevel = productDetailModel.VatLevel
        };

        public static Product MapListModelToEntity(ProductListModel productListModel) => new()
        {
            ID = productListModel.ID,
            Category = MapListModelToEntity(productListModel.Category),
            Name = productListModel.Name,
            Price = productListModel.Price,
            VatLevel = productListModel.VatLevel
        };

        public static Sell MapDetailModelToEntity(SellDetailModel sellDetailModel) => new()
        {
            DateTime = sellDetailModel.DateTime,
            Discount = sellDetailModel.Discount,
            ID = sellDetailModel.ID,
            Product = MapListModelToEntity(sellDetailModel.Product),
            Table = MapListModelToEntity(sellDetailModel.Table),
            Payment = MapListModelToEntity(sellDetailModel.Payment)
        };

        public static Sell MapListModelToEntity(SellDetailModel sellListModel) => MapDetailModelToEntity(sellListModel);

        public static Shift MapDetailModelToEntity(ShiftDetailModel shiftDetailModel) => new()
        {
            ID = shiftDetailModel.ID,
            CashValue = shiftDetailModel.CashValue,
            DateTime = shiftDetailModel.DateTime,
            ShiftRecordType = shiftDetailModel.ShiftRecordType,
            User = MapListModelToEntity(shiftDetailModel.User)
        };

        public static Shift MapListModelToEntity(ShiftListModel shiftListModel) => new()
        {
            ID = shiftListModel.ID,
            DateTime = shiftListModel.DateTime,
            ShiftRecordType = shiftListModel.ShiftRecordType
        };

        public static TableCategory MapDetailModelToEntity(TableCategoryModel tableCategoryModel) => new()
        {
            ID = tableCategoryModel.ID,
            Name = tableCategoryModel.Name
        };

        public static TableCategory MapListModelToEntity(TableCategoryModel tableCategory) => MapDetailModelToEntity(tableCategory);

        public static TableInfo MapDetailModelToEntity(TableInfoDetailModel tableInfoDetailModel) => new()
        {
            ID = tableInfoDetailModel.ID,
            Name = tableInfoDetailModel.Name,
            Category = MapListModelToEntity(tableInfoDetailModel.Category),
            PositionX = tableInfoDetailModel.PositionX,
            PositionY = tableInfoDetailModel.PositionY,
            Size = tableInfoDetailModel.Size
        };

        public static TableInfo MapListModelToEntity(TableInfoDetailModel tableInfoDetailModel) => MapDetailModelToEntity(tableInfoDetailModel);

        public static Table MapDetailModelToEntity(TableDetailModel tableDetailModel) => new()
        {
            ID = tableDetailModel.ID,
            EndDateTime = tableDetailModel.EndDateTime,
            Payments = new ObservableCollection<Payment>(tableDetailModel.Payments.Select(MapListModelToEntity)),
            Sells = new ObservableCollection<Sell>(tableDetailModel.Sells.Select(MapListModelToEntity)),
            StartDateTime = tableDetailModel.StartDateTime,
            TableInfo = MapListModelToEntity(tableDetailModel.TableInfo)
        };

        public static Table MapListModelToEntity(TableListModel tableListModel) => new()
        {
            ID = tableListModel.ID,
            StartDateTime = tableListModel.StartDateTime,
            TableInfo = MapListModelToEntity(tableListModel.TableInfo)
        };

        public static User MapDetailModelToEntity(UserDetailModel userDetailModel) => new()
        {
            ID = userDetailModel.ID,
            FirstName = userDetailModel.FirstName,
            Hash = userDetailModel.Hash,
            LastName = userDetailModel.LastName,
            Rights = userDetailModel.Rights,
            Salt = userDetailModel.Salt,
            NickName = userDetailModel.NickName
        };

        public static User MapListModelToEntity(UserListModel userListModel) => new()
        {
            ID = userListModel.ID,
            FirstName = userListModel.FirstName,
            LastName = userListModel.LastName,
            NickName = userListModel.NickName
        };

        public static Card MapDetailModelToEntity(CardDetailModel cardDetailModel) => MapDetailModelToEntity(cardDetailModel);

        public static Card MapListModelToEntity(CardListModel cardListModel) => new()
        {
            ID = cardListModel.ID,
            Number = cardListModel.Number
        };

        public static Reservation MapDetailModelToEntity(ReservationDetailModel reservationDetailModel) => new()
        {
            ID = reservationDetailModel.ID,
            Name = reservationDetailModel.Name,
            Email = reservationDetailModel.Email,
            StartTime = reservationDetailModel.StartTime,
            EndTime = reservationDetailModel.EndTime,
            SelectedTable = MapListModelToEntity(reservationDetailModel.SelectedTable),
            PeopleCount = reservationDetailModel.PeopleCount
        };

        public static Reservation MapListModelToEntity(ReservationListModel reservationListModel) => new()
        {
            ID = reservationListModel.ID,
            Name = reservationListModel.Name,
            StartTime = reservationListModel.StartTime,
            EndTime = reservationListModel.EndTime
        };

        public static ExpenseIncome MapDetailModelToEntity(ExpenseIncomeDetailModel expenseIncomeDetailModel) => new()
        {
            ID = expenseIncomeDetailModel.ID,
            DateTime = expenseIncomeDetailModel.DateTime,
            EIType = expenseIncomeDetailModel.EIType,
            Price = expenseIncomeDetailModel.Price,
            User = MapListModelToEntity(expenseIncomeDetailModel.User)
        };

        public static ExpenseIncome MapListModelToEntity(ExpenseIncomeListModel expenseIncomeListModel) => new()
        {
            ID = expenseIncomeListModel.ID,
            DateTime = expenseIncomeListModel.DateTime,
            EIType = expenseIncomeListModel.EIType,
            Price = expenseIncomeListModel.Price
        };

        public static UserLog MapDetailModelToEntity(UserLogDetailModel userLogDetailModel) => new()
        {
            ID = userLogDetailModel.ID,
            DateTime = userLogDetailModel.DateTime,
            LogType = userLogDetailModel.LogType,
            User = MapListModelToEntity(userLogDetailModel.User)
        };

        public static UserLog MapListModelToEntity(UserLogDetailModel userLogDetailModel) => MapDetailModelToEntity(userLogDetailModel);
    }
}
