using CloudCash.BL.DbAccess.Base;
using CloudCash.BL.DTOs.TableInfo;
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
    public class DbTableInfo : DbBase
    {
        public DbTableInfo(IDbContextFactory cloudCashDbContextFactory) : base(cloudCashDbContextFactory) { }

        public bool AddTableInfo(TableInfoDetailModel tableInfoDetailModel)
        {
            _connection.ChangeTracker.Clear();

            var entity = Mapper.MapDetailModelToEntity(tableInfoDetailModel);

            entity.Category = _connection.TableCategories.FirstOrDefault(x => x.ID == tableInfoDetailModel.Category.ID);

            _connection.TableInfos.Add(entity);

            return _connection.SaveChanges() is 1;
        }

        public bool RemoveTableInfo(TableInfoDetailModel tableInfoDetailModel)
        {
            _connection.ChangeTracker.Clear();

            _connection.TableInfos.Remove(Mapper.MapDetailModelToEntity(tableInfoDetailModel));
            return _connection.SaveChanges() is 1;
        }

        public bool EditTableInfo(TableInfoDetailModel tableInfoDetailModel)
        {
            _connection.ChangeTracker.Clear();

            var entityTableInfo = _connection.TableInfos.First(x => x.ID == tableInfoDetailModel.ID);
            var mappedEntityTableInfo = Mapper.MapDetailModelToEntity(tableInfoDetailModel);

            if (entityTableInfo != mappedEntityTableInfo)
            {
                _connection.Entry(entityTableInfo).CurrentValues.SetValues(mappedEntityTableInfo);

                return _connection.SaveChanges() is 1;
            }
            else
                return false;
        }

        public int GetNumberOfTablesInCategory(long categoryId) => GetTableInfoWithIncludesData(_connection).Count(x => x.Category.ID == categoryId);

        public async Task<ObservableCollection<TableInfoDetailModel>> GetTableInfos() => await GetTableInfosCollection(TableInfoPredicate(), Mapper.MapEntityToDetailModel);

        public TableInfoDetailModel GetTableInfoByID(long id) => Mapper.MapEntityToDetailModel(GetTableInfoWithIncludesData(_connection).First(x => x.ID == id));

        private async Task<ObservableCollection<T>> GetTableInfosCollection<T>(Func<TableInfo, bool> filterPredicate, Func<TableInfo, T> mapEntityToModel) where T : TableInfoDetailModel
        {
            return new(await Task.Run(() =>
            {
                var connection = _cloudCashDbContextFactory.CreateDbContext();

                return GetTableInfoWithIncludesData(connection)?.AsQueryable().Where(filterPredicate).Select(mapEntityToModel).ToList() ?? null;
            }));
        }

        private static Func<TableInfo, bool> TableInfoPredicate() => x => true;

        private IIncludableQueryable<TableInfo, TableCategory> GetTableInfoWithIncludesData(CloudCashDbContext connection) => connection.TableInfos.Include(x => x.Category);
    }
}
