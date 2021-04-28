using CloudCash.BL.DbAccess.Base;
using CloudCash.BL.DTOs.Payments;
using CloudCash.BL.DTOs.TableCategories;
using CloudCash.BL.DTOs.TableInfo;
using CloudCash.BL.DTOs.Tables;
using CloudCash.DAL.Data;
using CloudCash.DAL.Entities;
using CloudCash.DAL.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CloudCash.BL.DbAccess
{
    public class DbTable : DbBase
    {
        public DbTable(IDbContextFactory cloudCashDbContextFactory) : base(cloudCashDbContextFactory) { }

        public bool AddTable(TableDetailModel tableDetailModel)
        {
            _connection.ChangeTracker.Clear();

            var entity = Mapper.MapDetailModelToEntity(tableDetailModel);

            entity.TableInfo = _connection.TableInfos.FirstOrDefault(x => x.ID == tableDetailModel.TableInfo.ID);

            _connection.Tables.Add(entity);

            var res = _connection.SaveChanges() is 1;

            if (res)
                tableDetailModel.ID = entity.ID;

            return res;
        }

        public bool RemoveTable(TableDetailModel tableDetailModel)
        {
            _connection.ChangeTracker.Clear();

            _connection.Tables.Remove(Mapper.MapDetailModelToEntity(tableDetailModel));
            return _connection.SaveChanges() is 1;
        }

        public bool EditTable(TableDetailModel tableDetailModel)
        {
            _connection.ChangeTracker.Clear();

            var entityTable = _connection.Tables.First(x => x.ID == tableDetailModel.ID);
            var mappedEntityTable = Mapper.MapDetailModelToEntity(tableDetailModel);

            if (entityTable != mappedEntityTable)
            {
                _connection.Entry(entityTable).CurrentValues.SetValues(mappedEntityTable);

                return _connection.SaveChanges() is 1;
            }
            else
                return false;
        }

        public bool CloseTable(TableDetailModel tableDetailModel)
        {
            tableDetailModel.EndDateTime = DateTime.Now;

            return EditTable(tableDetailModel);
        }

        public bool AddPayment(TableDetailModel tableDetailModel, PaymentDetailModel paymentDetailModel)
        {
            // todo
            _connection.ChangeTracker.Clear();

            var entityPayment = _connection.Payments.First(x => x.ID == paymentDetailModel.ID);
            var entityEntityTable = _connection.Tables.First(x => x.ID == tableDetailModel.ID);

            if (!entityEntityTable.Payments.Contains(entityPayment))
            {
                if (entityEntityTable.Payments.Count is 1)
                {
                    entityEntityTable.Payments.ToList()[0].IsPartial = true;
                    entityPayment.IsPartial = true;
                }

                entityEntityTable.Payments.Add(Mapper.MapDetailModelToEntity(paymentDetailModel));

                return _connection.SaveChanges() is 2;
            }
            else
                return false;
        }

        public TableDetailModel GetOpenedTable(TableInfoDetailModel tableInfo) => GetOpenedTableById(tableInfo.ID);

        public TableDetailModel GetOpenedTableById(long id) => Mapper.MapEntityToDetailModel(GetTableWithIncludesData(_connection).First(x => x.TableInfo.ID == id && x.EndDateTime == DateTime.MinValue));

        public TableDetailModel GetTableById(long id) => Mapper.MapEntityToDetailModel(GetTableWithIncludesData(_connection).First(x => x.ID == id));

        public Task<ObservableCollection<TableDetailModel>> GetOpenedTables() => GetTablesCollection(OpenedTablesPredicate(), Mapper.MapEntityToDetailModel);

        public Task<ObservableCollection<TableListModel>> GetOpenedTablesList() => GetTablesCollection(OpenedTablesPredicate(), Mapper.MapEntityToListModel);

        public Task<ObservableCollection<TableDetailModel>> GetTables() => GetTablesCollection(TablesPredicate(), Mapper.MapEntityToDetailModel);

        public Task<ObservableCollection<TableListModel>> GetTablesList() => GetTablesCollection(TablesPredicate(), Mapper.MapEntityToListModel);

        public Task<ObservableCollection<TableDetailModel>> GetTablesByCategory(TableCategoryModel tableCategoryModel) => GetTablesCollection(TablesByCategoryPredicate(tableCategoryModel), Mapper.MapEntityToDetailModel);

        public Task<ObservableCollection<TableListModel>> GetTablesByCategoryList(TableCategoryModel tableCategoryModel) => GetTablesCollection(TablesByCategoryPredicate(tableCategoryModel), Mapper.MapEntityToListModel);

        //public ObservableCollection<TableDetailModel> GetTablesByCategory(TableCategoryModel tableCategoryModel)
        //{
        //    using var connection = _cloudCashDbContextFactory.CreateDbContext();

        //    return new ObservableCollection<T>(connection.Tables.Where(x => x.TableInfo.Category).Select(mapEntityToModel));
        //}

        //public ObservableCollection<TableListModel> GetTablesByCategoryList(TableCategoryModel tableCategoryModel)
        //{

        //}

        private async Task<ObservableCollection<T>> GetTablesCollection<T>(Func<Table, bool> filterPredicate, Func<Table, T> mapEntityToModel) where T : TableListModel
        {
            return new(await Task.Run(() =>
            {
                var connection = _cloudCashDbContextFactory.CreateDbContext();

                return GetTableWithIncludesData(connection)?.AsQueryable().Where(filterPredicate).Select(mapEntityToModel).ToList() ?? null;
            }));
        }

        private static Func<Table, bool> OpenedTablesPredicate() => x => x.EndDateTime == DateTime.MinValue;

        private Func<Table, bool> TablesByCategoryPredicate(TableCategoryModel tableCategoryModel) =>
            x => x.TableInfo.Category.ID == tableCategoryModel.ID;

        private static Func<Table, bool> TablesPredicate() => x => true;

        private IIncludableQueryable<Table, ICollection<Payment>> GetTableWithIncludesData(CloudCashDbContext connection) => connection.Tables.Include(x => x.TableInfo).ThenInclude(x => x.Category).Include(x => x.Sells).ThenInclude(x => x.Product).ThenInclude(x => x.Category).Include(x => x.Payments);
    }
}
