using CloudCash.BL.DbAccess.Base;
using CloudCash.BL.DTOs.Customers;
using CloudCash.BL.DTOs.Payments;
using CloudCash.BL.DTOs.Shifts;
using CloudCash.Common.Enums;
using CloudCash.DAL.Data;
using CloudCash.DAL.Entities;
using CloudCash.DAL.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CloudCash.BL.DbAccess
{
    public class DbPayment : DbBase
    {
        public DbPayment(IDbContextFactory cloudCashDbContextFactory) : base(cloudCashDbContextFactory) { }

        public bool AddPayment(PaymentDetailModel paymentDetailModel)
        {
            _connection.ChangeTracker.Clear();

            var entity = Mapper.MapDetailModelToEntity(paymentDetailModel);

            entity.Table = _connection.Tables.First(x => x.ID == paymentDetailModel.Table.ID);
            entity.Sells.ToList().ForEach(x =>
            {
                x.Payment = entity;
                _connection.Entry(x).State = EntityState.Modified;
            });

            entity.Customer = _connection.Customers.FirstOrDefault(x => x.ID == paymentDetailModel.Customer.ID);

            _connection.Payments.Add(entity);
            return _connection.SaveChanges() is 1;
        }

        public bool RemovePayment(PaymentDetailModel paymentDetailModel)
        {
            _connection.Payments.Remove(Mapper.MapDetailModelToEntity(paymentDetailModel));
            return _connection.SaveChanges() is 1;
        }

        public bool EditPayment(PaymentDetailModel paymentDetailModel)
        {
            var entityPayment = _connection.Payments.First(x => x.ID == paymentDetailModel.ID);
            var mappedEntityPayment = Mapper.MapEntityToDetailModel(entityPayment);

            if (mappedEntityPayment != paymentDetailModel)
            {
                entityPayment = Mapper.MapDetailModelToEntity(paymentDetailModel);
                return _connection.SaveChanges() is 1;
            }
            else
                return false;
        }

        public PaymentDetailModel GetPaymentByID(uint id)
        {
            var payment = _connection.Payments.First(x => x.ID == id);
            return payment is null ? null : Mapper.MapEntityToDetailModel(payment);
        }

        public Task<ObservableCollection<PaymentDetailModel>> GetPayments() => GetPaymentsCollection(PaymentsPredicate(), Mapper.MapEntityToDetailModel);

        public Task<ObservableCollection<PaymentListModel>> GetPaymentsList() => GetPaymentsCollection(PaymentsPredicate(), Mapper.MapEntityToListModel);

        public Task<ObservableCollection<PaymentListModel>> GetPaymentsByShiftList(ShiftDetailModel shiftDetailModel) => GetPaymentsCollection(PaymentsByShift(shiftDetailModel), Mapper.MapEntityToListModel);

        public Task<ObservableCollection<PaymentListModel>> GetPaymentsByCustomerList(CustomerDetailModel customerDetailModel) =>
            GetPaymentsCollection(PaymentsByCustomerPredicate(customerDetailModel), Mapper.MapEntityToListModel);

        public Task<ObservableCollection<PaymentDetailModel>> GetPaymentsByCustomer(CustomerDetailModel customerDetailModel) =>
            GetPaymentsCollection(PaymentsByCustomerPredicate(customerDetailModel), Mapper.MapEntityToDetailModel);

        public Task<ObservableCollection<PaymentListModel>> GetPaymentsByPaymentTypeList(PaymentType paymentType) =>
            GetPaymentsCollection(PaymentsByPaymentTypePredicate(paymentType), Mapper.MapEntityToListModel);

        public Task<ObservableCollection<PaymentDetailModel>> GetPaymentsByPaymentType(PaymentType paymentType) =>
            GetPaymentsCollection(PaymentsByPaymentTypePredicate(paymentType), Mapper.MapEntityToDetailModel);

        private async Task<ObservableCollection<T>> GetPaymentsCollection<T>(Func<Payment, bool> filterPredicate, Func<Payment, T> mapEntityToModel) where T : PaymentListModel
        {
            return new(await Task.Run(() =>
            {
                var connection = _cloudCashDbContextFactory.CreateDbContext();

                return GetPaymentWithIncludesData(connection)?.AsQueryable().Where(filterPredicate).Select(mapEntityToModel).ToList() ?? null;
            }));
        }

        private static Func<Payment, bool> PaymentsPredicate() => x => true;

        private static Func<Payment, bool> PaymentsByCustomerPredicate(CustomerDetailModel customerDetailModel) =>
            x => x.Customer.ID == customerDetailModel.ID;

        private static Func<Payment, bool> PaymentsByPaymentTypePredicate(PaymentType paymentType) =>
            x => x.PaymentType == paymentType;

        private static Func<Payment, bool> PaymentsByShift(ShiftDetailModel shiftDetailModel) =>
            x => x.DateTime > shiftDetailModel.DateTime;

        private IIncludableQueryable<Payment, Payment> GetPaymentWithIncludesData(CloudCashDbContext _connection) => _connection.Payments.Include(x => x.Table).ThenInclude(x => x.TableInfo).ThenInclude(x => x.Category).Include(x => x.Customer).ThenInclude(x => x.Card).Include(x => x.Sells).ThenInclude(x => x.Table).ThenInclude(x => x.TableInfo).ThenInclude(x => x.Category).Include(x => x.Sells).ThenInclude(x => x.Product).ThenInclude(x => x.Category).Include(x => x.Sells).ThenInclude(x => x.Payment);
    }
}
