using CloudCash.BL.DbAccess.Base;
using CloudCash.BL.DTOs.Products;
using CloudCash.BL.DTOs.Sells;
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
    public class DbSell : DbBase
    {
        public DbSell(IDbContextFactory cloudCashDbContextFactory) : base(cloudCashDbContextFactory) { }

        public bool AddSell(SellDetailModel sellDetailModel)
        {
            _connection.ChangeTracker.Clear();

            var entity = AddSellCore(sellDetailModel);

            var res = _connection.SaveChanges() is 1;

            if (res)
                sellDetailModel.ID = entity.ID;

            return res;
        }

        public bool AddSellRange(List<SellDetailModel> sellDetailModels)
        {
            _connection.ChangeTracker.Clear();

            foreach (var sell in sellDetailModels)
                AddSellCore(sell);

            return _connection.SaveChanges() >= 1;
        }

        private Sell AddSellCore(SellDetailModel sellDetailModel)
        {
            sellDetailModel.ID = 0;
            var entity = Mapper.MapDetailModelToEntity(sellDetailModel);

            entity.Table = _connection.Tables.FirstOrDefault(x => x.ID == sellDetailModel.Table.ID);
            entity.Product = _connection.Products.FirstOrDefault(x => x.ID == sellDetailModel.Product.ID);
            entity.Payment = _connection.Payments.FirstOrDefault(x => x.ID == sellDetailModel.Payment.ID);

            _connection.Sells.Add(entity);
            return entity;
        }

        public bool RemoveSell(SellDetailModel sellDetailModel) => RemoveSellEntity(Mapper.MapDetailModelToEntity(sellDetailModel));

        public bool RemoveSellById(long sellId) => RemoveSellEntity(GetSellByIdEntity(sellId));

        private bool RemoveSellEntity(Sell sellToDelete)
        {
            _connection.ChangeTracker.Clear();

            _connection.Sells.Remove(sellToDelete);
            return _connection.SaveChanges() is 1;
        }

        public bool RemoveSellRange(List<SellDetailModel> sellsToDelete)
        {
            _connection.ChangeTracker.Clear();

            List<Sell> sellsToDeleteEntities = new();

            foreach (var sell in sellsToDelete)
            {
                var sellEntity = Mapper.MapDetailModelToEntity(sell);
                sellEntity.Product = _connection.Products.First(x => x.ID == sell.Product.ID);
                sellEntity.Table = _connection.Tables.First(x => x.ID == sell.Table.ID);

                sellsToDeleteEntities.Add(sellEntity);
            }

            _connection.Sells.RemoveRange(sellsToDeleteEntities);

            return _connection.SaveChanges() is 1;
        }

        public bool RemoveSellsFromTable(SellDetailModel referenceSell)
        {
            _connection.ChangeTracker.Clear();

            var sellsRangeToRemove = _connection.Sells.Where(x => x.Table.ID == referenceSell.Table.ID && x.Discount == referenceSell.Discount && x.Product.ID == referenceSell.Product.ID).ToList();

            _connection.Sells.RemoveRange(sellsRangeToRemove);
            return _connection.SaveChanges() is 1;
        }

        public bool EditSell(SellDetailModel sellDetailModel)
        {
            _connection.ChangeTracker.Clear();

            var entitySell = _connection.Sells.First(x => x.ID == sellDetailModel.ID);
            var mappedEntitySell = Mapper.MapDetailModelToEntity(sellDetailModel);

            if (entitySell != mappedEntitySell)
            {
                _connection.Entry(entitySell).CurrentValues.SetValues(mappedEntitySell);

                return _connection.SaveChanges() is 1;
            }
            else
                return false;
        }

        public bool AddTableToSell(SellDetailModel sellDetailModel, TableDetailModel tableDetailModel)
        {
            // todo
            _connection.ChangeTracker.Clear();

            var entityTable = _connection.Tables.First(x => x.ID == tableDetailModel.ID);
            var entitySell = _connection.Sells.First(x => x.ID == sellDetailModel.ID);

            if (entitySell.Table != entityTable)
            {
                entitySell.Table = entityTable;

                return _connection.SaveChanges() is 1;
            }
            else
                return false;
        }

        public bool AddProductToSell(SellDetailModel sellDetailModel, ProductDetailModel productDetailModel)
        {
            // todo
            _connection.ChangeTracker.Clear();

            var entityProduct = _connection.Products.First(x => x.ID == productDetailModel.ID);
            var entitySell = _connection.Sells.First(x => x.ID == sellDetailModel.ID);

            if (entitySell.Product != entityProduct)
            {
                entitySell.Product = entityProduct;

                return _connection.SaveChanges() is 1;
            }
            else
                return false;
        }

        public bool SetDiscount(SellDetailModel sellDetailModel, byte discount) => SetDiscountRange(new() { sellDetailModel }, discount);

        public bool SetDiscountRange(List<SellDetailModel> sellDetailModels, byte discount)
        {
            _connection.ChangeTracker.Clear();

            foreach (var sell in sellDetailModels)
            {
                sell.Discount = discount;
                var entitySell = _connection.Sells.First(x => x.ID == sell.ID);
                var mappedEntitySell = Mapper.MapDetailModelToEntity(sell);

                if (entitySell != mappedEntitySell)
                    _connection.Entry(entitySell).CurrentValues.SetValues(mappedEntitySell);
            }
            return _connection.SaveChanges() >= 1;
        }

        public SellDetailModel GetSellById(long id) => Mapper.MapEntityToDetailModel(GetSellByIdEntity(id));

        private Sell GetSellByIdEntity(long id) => GetSellWithIncludesData(_connection).First(x => x.ID == id);

        public Task<ObservableCollection<SellDetailModel>> GetSellsReload(long productId, long tableId, byte discount) => GetSellsCollection(ReloadSellsPredicate(productId, tableId, discount), Mapper.MapEntityToDetailModel);

        public Task<ObservableCollection<SellDetailModel>> GetSells() => GetSellsCollection(SellsPredicate(), Mapper.MapEntityToDetailModel);

        public Task<ObservableCollection<SellDetailModel>> GetSellsByTableId(long id) => GetSellsCollection(SellsByTableIdPredicate(id), Mapper.MapEntityToDetailModel);

        public Task<ObservableCollection<SellDetailModel>> GetUnpaidSellsByTableId(long id) => GetSellsCollection(UnpaiSellsByTableIdPredicate(id), Mapper.MapEntityToDetailModel);

        private async Task<ObservableCollection<T>> GetSellsCollection<T>(Func<Sell, bool> filterPredicate, Func<Sell, T> mapEntityToModel) where T : SellDetailModel
        {
            return new(await Task.Run(() =>
            {
                var connection = _cloudCashDbContextFactory.CreateDbContext();

                return GetSellWithIncludesData(connection)?.AsQueryable().Where(filterPredicate).Select(mapEntityToModel).ToList() ?? null;
            }));
        }

        private static Func<Sell, bool> SellsPredicate() => x => true;

        private static Func<Sell, bool> SellsByTableIdPredicate(long id) => x => x.Table.ID == id;

        private static Func<Sell, bool> UnpaiSellsByTableIdPredicate(long id) => x => x.Table.ID == id && x.Payment is null;

        private static Func<Sell, bool> ReloadSellsPredicate(long productId, long tableId, byte discount) => x => x.Product.ID == productId && x.Table.ID == tableId && x.Discount == discount;

        private IIncludableQueryable<Sell, TableCategory> GetSellWithIncludesData(CloudCashDbContext connection) => connection.Sells.Include(x => x.Payment).ThenInclude(x => x.Table).ThenInclude(x => x.TableInfo).ThenInclude(x => x.Category).Include(x => x.Product).ThenInclude(x => x.Category).Include(x => x.Table).ThenInclude(x => x.TableInfo).ThenInclude(x => x.Category);
    }
}
