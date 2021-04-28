using CloudCash.BL.DbAccess.Base;
using CloudCash.BL.DTOs.TableCategories;
using CloudCash.DAL.Factories;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CloudCash.BL.DbAccess
{
    public class DbTableCategory : DbBase
    {
        public DbTableCategory(IDbContextFactory cloudCashDbContextFactory) : base(cloudCashDbContextFactory) { }

        public bool AddTableCategory(TableCategoryModel tableCategoryModel)
        {
            _connection.ChangeTracker.Clear();

            _connection.TableCategories.Add(Mapper.MapDetailModelToEntity(tableCategoryModel));
            return _connection.SaveChanges() is 1;
        }

        public bool RemoveTableCategory(TableCategoryModel tableCategoryModel)
        {
            _connection.ChangeTracker.Clear();

            _connection.TableCategories.Remove(Mapper.MapDetailModelToEntity(tableCategoryModel));
            return _connection.SaveChanges() is 1;
        }

        public bool EditTableCategory(TableCategoryModel tableCategoryModel)
        {
            _connection.ChangeTracker.Clear();

            var entityTableCategory = _connection.TableCategories.First(x => x.ID == tableCategoryModel.ID);
            var mappedEntityTableCategory = Mapper.MapDetailModelToEntity(tableCategoryModel);

            if (entityTableCategory != mappedEntityTableCategory)
            {
                _connection.Entry(entityTableCategory).CurrentValues.SetValues(mappedEntityTableCategory);

                return _connection.SaveChanges() is 1;
            }
            else
                return false;
        }

        public TableCategoryModel GetTableCategoryByName(string name) => Mapper.MapEntityToDetailModel(_connection.TableCategories.First(x => x.Name == name));

        public TableCategoryModel GetTableCategoryById(long id) => Mapper.MapEntityToDetailModel(_connection.TableCategories.First(x => x.ID == id));

        public async Task<ObservableCollection<TableCategoryModel>> GetTableCategories()
        {
            return new(await Task.Run(() =>
            {
                var connection = _cloudCashDbContextFactory.CreateDbContext();

                return connection.TableCategories.AsQueryable().Select(Mapper.MapEntityToDetailModel).ToList();
            }));
        }
    }
}
