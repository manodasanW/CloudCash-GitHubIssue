using CloudCash.BL.DbAccess.Base;
using CloudCash.BL.DTOs.UserLog;
using CloudCash.DAL.Entities;
using CloudCash.DAL.Factories;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CloudCash.BL.DbAccess
{
    public class DbUserLog : DbBase
    {
        public DbUserLog(IDbContextFactory cloudCashDbContextFactory) : base(cloudCashDbContextFactory) { }

        public bool AddUserLog(UserLogDetailModel userLogDetailModel)
        {
            _connection.ChangeTracker.Clear();

            var entity = Mapper.MapDetailModelToEntity(userLogDetailModel);

            entity.User = _connection.Users.FirstOrDefault(x => x.ID == userLogDetailModel.User.ID);

            _connection.UserLogs.Add(entity);

            return _connection.SaveChanges() is 1;
        }

        public async Task<ObservableCollection<UserLogDetailModel>> GetUserLogsList() => await GetUserLogsCollection(UserLogsPredicate(), Mapper.MapEntityToDetailModel);

        private async Task<ObservableCollection<T>> GetUserLogsCollection<T>(Func<UserLog, bool> filterPredicate, Func<UserLog, T> mapEntityToModel) where T : UserLogDetailModel
        {
            return new(await Task.Run(() =>
            {
                var connection = _cloudCashDbContextFactory.CreateDbContext();

                return connection.UserLogs.AsQueryable().Where(filterPredicate).Select(mapEntityToModel).ToList() ?? null;
            }));
        }

        private static Func<UserLog, bool> UserLogsPredicate() => x => true;
    }
}
