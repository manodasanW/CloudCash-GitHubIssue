using CloudCash.BL.DTOs.Cards;
using CloudCash.BL.DTOs.Customers;
using CloudCash.DAL.Entities;
using CloudCash.DAL.Factories;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace CloudCash.BL.DbAccess
{
    public class DbCustomer
    {
        private readonly IDbContextFactory _cloudCashDbContextFactory;

        public DbCustomer(IDbContextFactory cloudCashDbContextFactory)
        {
            _cloudCashDbContextFactory = cloudCashDbContextFactory;
        }

        public bool AddCustomer(CustomerDetailModel customerDetailModel)
        {
            using var connection = _cloudCashDbContextFactory.CreateDbContext();

            connection.Customers.Add(Mapper.MapDetailModelToEntity(customerDetailModel));
            return connection.SaveChanges() is 1;
        }

        public bool RemoveCustomer(CustomerDetailModel customerDetailModel)
        {
            using var connection = _cloudCashDbContextFactory.CreateDbContext();
            connection.Customers.Remove(Mapper.MapDetailModelToEntity(customerDetailModel));
            return connection.SaveChanges() is 1;
        }

        public bool EditCustomer(CustomerDetailModel customerDetailModel)
        {
            using var connection = _cloudCashDbContextFactory.CreateDbContext();

            var entityCustomer = connection.Customers.First(x => x.ID == customerDetailModel.ID);
            var mappedEntityCustomer = Mapper.MapEntityToDetailModel(entityCustomer);

            if (mappedEntityCustomer != customerDetailModel)
            {
                entityCustomer = Mapper.MapDetailModelToEntity(customerDetailModel);
                return connection.SaveChanges() is 1;
            }
            else
                return false;
        }

        public CustomerDetailModel GetCustomerByID(uint id)
        {
            using var connection = _cloudCashDbContextFactory.CreateDbContext();

            var customer = connection.Customers.FirstOrDefault(x => x.ID == id);
            return customer is null ? null : Mapper.MapEntityToDetailModel(customer);
        }

        public CustomerDetailModel GetCustomerByCard(CardListModel cardListModel)
        {
            using var connection = _cloudCashDbContextFactory.CreateDbContext();

            var customer = connection.Customers.FirstOrDefault(x => x.Card.ID == cardListModel.ID);
            return customer is null ? null : Mapper.MapEntityToDetailModel(customer);
        }

        public CustomerDetailModel GetCustomerByEmail(string email)
        {
            using var connection = _cloudCashDbContextFactory.CreateDbContext();

            var customer = connection.Customers.FirstOrDefault(x => x.Email == email);
            return customer is null ? null : Mapper.MapEntityToDetailModel(customer);
        }

        public ObservableCollection<CustomerDetailModel> GetCustomers() => GetCustomersCollection(CustomersPredicate(), Mapper.MapEntityToDetailModel);

        public ObservableCollection<CustomerListModel> GetCustomersList() => GetCustomersCollection(CustomersPredicate(), Mapper.MapEntityToListModel);

        private ObservableCollection<T> GetCustomersCollection<T>(Func<Customer, bool> filterPredicate, Func<Customer, T> mapEntityToModel) where T : CustomerListModel
        {
            using var connection = _cloudCashDbContextFactory.CreateDbContext();

            return new ObservableCollection<T>(connection.Customers.AsQueryable().Where(filterPredicate).Select(mapEntityToModel));
        }

        private static Func<Customer, bool> CustomersPredicate() => x => true;
    }
}
