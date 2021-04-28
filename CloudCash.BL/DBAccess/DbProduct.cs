using CloudCash.BL.DTOs.ProductCategories;
using CloudCash.BL.DTOs.Products;
using CloudCash.DAL.Entities;
using CloudCash.DAL.Factories;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using CloudCash.DAL.Data;
using CloudCash.BL.DbAccess.Base;

namespace CloudCash.BL.DbAccess
{
    public class DbProduct : DbBase
    {
        public DbProduct(IDbContextFactory cloudCashDbContextFactory) : base(cloudCashDbContextFactory) { }

        public bool AddProduct(ProductDetailModel productDetailModel)
        {
            _connection.ChangeTracker.Clear();

            var entity = Mapper.MapDetailModelToEntity(productDetailModel);
            
            entity.Category = _connection.ProductCategories.FirstOrDefault(x => x.ID == productDetailModel.Category.ID);

            _connection.Products.Add(entity);

            return _connection.SaveChanges() is 1;
        }

        public bool RemoveProduct(ProductDetailModel productDetailModel) => RemoveProductEntity(Mapper.MapDetailModelToEntity(productDetailModel));

        private bool RemoveProductEntity(Product product)
        {
            _connection.ChangeTracker.Clear();

            _connection.Products.Remove(product);
            return _connection.SaveChanges() is 1;
        }

        public bool RemoveProductById(long id) => RemoveProductEntity(GetProductByIdEntity(id));

        public bool EditProduct(ProductDetailModel productDetailModel)
        {
            _connection.ChangeTracker.Clear();

            var entityProduct = _connection.Products.First(x => x.ID == productDetailModel.ID);
            var mappedEntityProduct = Mapper.MapDetailModelToEntity(productDetailModel);

            if (entityProduct != mappedEntityProduct)
            {
                _connection.Entry(entityProduct).CurrentValues.SetValues(mappedEntityProduct);
                return _connection.SaveChanges() is 1;
            }
            else
                return false;
        }

        public ProductDetailModel GetProductByID(long id) => Mapper.MapEntityToDetailModel(GetProductByIdEntity(id));

        private Product GetProductByIdEntity(long id) => GetProductWithIncludesData(_connection).First(x => x.ID == id);

        public int GetNumberOfProductsInCategory(long categoryId) => GetProductWithIncludesData(_connection).Count(x => x.Category.ID == categoryId);

        public async Task<ObservableCollection<ProductDetailModel>> GetProducts() => await GetProductsCollection(ProductsPredicate(), Mapper.MapEntityToDetailModel);

        public async Task<ObservableCollection<ProductListModel>> GetProductsList() => await GetProductsCollection(ProductsPredicate(), Mapper.MapEntityToListModel);

        public async Task<ObservableCollection<ProductListModel>> GetProductsByCategory(ProductCategoryModel productCategoryModel) =>
            await GetProductsCollection(ProductsByCategoryPredicate(productCategoryModel), Mapper.MapEntityToListModel);

        public async Task<ObservableCollection<ProductListModel>> GetProductsByCategoryList(ProductCategoryModel productCategoryModel) =>
            await GetProductsCollection(ProductsByCategoryPredicate(productCategoryModel), Mapper.MapEntityToListModel);

        private async Task<ObservableCollection<T>> GetProductsCollection<T>(Func<Product, bool> filterPredicate, Func<Product, T> mapEntityToModel) where T : ProductListModel
        {
            return new(await Task.Run(() =>
            {
                var connection = _cloudCashDbContextFactory.CreateDbContext();

                return GetProductWithIncludesData(connection)?.AsQueryable().Where(filterPredicate).Select(mapEntityToModel).ToList() ?? null;
            }));
        }

        private static Func<Product, bool> ProductsPredicate() => x => true;

        private static Func<Product, bool> ProductsByCategoryPredicate(ProductCategoryModel productCategoryModel) => x => x.Category.ID == productCategoryModel.ID;

        private IIncludableQueryable<Product, ProductCategory> GetProductWithIncludesData(CloudCashDbContext connection) => connection.Products.Include(x => x.Category);
    }
}
