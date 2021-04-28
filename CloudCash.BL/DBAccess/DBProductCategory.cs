using CloudCash.BL.DTOs.ProductCategories;
using CloudCash.DAL.Factories;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CloudCash.BL.DbAccess.Base;

namespace CloudCash.BL.DbAccess
{
    public class DbProductCategory : DbBase
    {
        public DbProductCategory(IDbContextFactory cloudCashDbContextFactory) : base(cloudCashDbContextFactory) { }

        public bool AddProductCategory(ProductCategoryModel productCategoryModel)
        {
            _connection.ChangeTracker.Clear();

            _connection.ProductCategories.Add(Mapper.MapDetailModelToEntity(productCategoryModel));
            return _connection.SaveChanges() is 1;
        }

        public bool RemoveProductCategory(ProductCategoryModel productCategoryModel)
        {
            _connection.ChangeTracker.Clear();

            _connection.ProductCategories.Remove(Mapper.MapDetailModelToEntity(productCategoryModel));
            return _connection.SaveChanges() is 1;
        }

        public bool EditProductCategory(ProductCategoryModel productCategoryModel)
        {
            _connection.ChangeTracker.Clear();

            var entityProductCategory = _connection.ProductCategories.First(x => x.ID == productCategoryModel.ID);
            var mappedEntityProductCategory = Mapper.MapDetailModelToEntity(productCategoryModel);

            if (entityProductCategory != mappedEntityProductCategory)
            {
                _connection.Entry(entityProductCategory).CurrentValues.SetValues(mappedEntityProductCategory);

                return _connection.SaveChanges() is 1;
            }
            else
                return false;
        }

        public ProductCategoryModel GetProductCategoryByName(string name) => Mapper.MapEntityToDetailModel(_connection.ProductCategories.First(x => x.Name == name));

        public ProductCategoryModel GetProductCategoryById(long id) => Mapper.MapEntityToDetailModel(_connection.ProductCategories.First(x => x.ID == id));

        public async Task<ObservableCollection<ProductCategoryModel>> GetProductCategories()
        {
            return new(await Task.Run(() =>
            {
                var connection = _cloudCashDbContextFactory.CreateDbContext();

                return connection.ProductCategories.AsQueryable().Select(Mapper.MapEntityToDetailModel).ToList();
            }));
        }
    }
}
