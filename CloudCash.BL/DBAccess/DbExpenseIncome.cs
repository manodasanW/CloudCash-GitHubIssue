using CloudCash.BL.DTOs.ExpenseIncomes;
using CloudCash.BL.DTOs.Users;
using CloudCash.DAL.Entities;
using CloudCash.DAL.Factories;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace CloudCash.BL.DbAccess
{
    public class DbExpenseIncome
    {
        private readonly IDbContextFactory _cloudCashDbContextFactory;

        public DbExpenseIncome(IDbContextFactory cloudCashDbContextFactory)
        {
            _cloudCashDbContextFactory = cloudCashDbContextFactory;
        }

        public bool AddExpenseIncome(ExpenseIncomeDetailModel eIDetailModel)
        {
            using var connection = _cloudCashDbContextFactory.CreateDbContext();

            connection.ExpenseIncomes.Add(Mapper.MapDetailModelToEntity(eIDetailModel));
            return connection.SaveChanges() is 1;
        }

        public bool RemoveExpenseIncome(ExpenseIncomeDetailModel eIDetailModel)
        {
            using var connection = _cloudCashDbContextFactory.CreateDbContext();
            connection.ExpenseIncomes.Remove(Mapper.MapDetailModelToEntity(eIDetailModel));
            return connection.SaveChanges() is 1;
        }

        public bool EditExpenseIncome(ExpenseIncomeDetailModel eIDetailModel)
        {
            using var connection = _cloudCashDbContextFactory.CreateDbContext();

            var entityExpenseIncome = connection.ExpenseIncomes.First(x => x.ID == eIDetailModel.ID);
            var mappedEntityExpenseIncome = Mapper.MapEntityToDetailModel(entityExpenseIncome);

            if (mappedEntityExpenseIncome != eIDetailModel)
            {
                entityExpenseIncome = Mapper.MapDetailModelToEntity(eIDetailModel);
                return connection.SaveChanges() is 1;
            }
            else
                return false;
        }

        public ExpenseIncomeDetailModel GetExpenseIncomeByID(uint id)
        {
            using var connection = _cloudCashDbContextFactory.CreateDbContext();

            var expenseIncome = connection.ExpenseIncomes.First(x => x.ID == id);
            return expenseIncome is null ? null : Mapper.MapEntityToDetailModel(expenseIncome);
        }

        public ObservableCollection<ExpenseIncomeDetailModel> GetExpenseIncomes() => GetExpenseIncomeCollection(ExpenseIncomePredicate(), Mapper.MapEntityToDetailModel);

        public ObservableCollection<ExpenseIncomeListModel> GetExpenseIncomesList() => GetExpenseIncomeCollection(ExpenseIncomePredicate(), Mapper.MapEntityToListModel);

        public ObservableCollection<ExpenseIncomeListModel> GetExpenseIncomesByUserList(UserDetailModel userDetailModel) =>
            GetExpenseIncomeCollection(ExpenseIncomeByUserPredicate(userDetailModel), Mapper.MapEntityToListModel);

        public ObservableCollection<ExpenseIncomeDetailModel> GetExpenseIncomesByUser(UserDetailModel userDetailModel) =>
            GetExpenseIncomeCollection(ExpenseIncomeByUserPredicate(userDetailModel), Mapper.MapEntityToDetailModel);

        private ObservableCollection<T> GetExpenseIncomeCollection<T>(Func<ExpenseIncome, bool> filterPredicate, Func<ExpenseIncome, T> mapEntityToModel) where T : ExpenseIncomeListModel
        {
            using var connection = _cloudCashDbContextFactory.CreateDbContext();

            return new ObservableCollection<T>(connection.ExpenseIncomes.AsQueryable().Where(filterPredicate).Select(mapEntityToModel));
        }

        private static Func<ExpenseIncome, bool> ExpenseIncomePredicate() => x => true;

        private static Func<ExpenseIncome, bool> ExpenseIncomeByUserPredicate(UserDetailModel userDetailModel) =>
            x => x.User.ID == userDetailModel.ID;
    }
}
