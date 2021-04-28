using CloudCash.BL.DbAccess.Base;
using CloudCash.BL.DTOs.Users;
using CloudCash.Common.Functions;
using CloudCash.DAL.Entities;
using CloudCash.DAL.Factories;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CloudCash.BL.DbAccess
{
    public class DbUser : DbBase
    {
        public DbUser(IDbContextFactory cloudCashDbContextFactory) : base(cloudCashDbContextFactory) { }

        public bool AddUser(UserDetailModel userDetailModel)
        {
            _connection.ChangeTracker.Clear();

            _connection.Users.Add(Mapper.MapDetailModelToEntity(userDetailModel));
            return _connection.SaveChanges() is 1;
        }

        public bool RemoveUser(UserDetailModel userDetailModel) => RemoveUserEntity(Mapper.MapDetailModelToEntity(userDetailModel));

        public bool RemoveUserEntity(User user)
        {
            _connection.ChangeTracker.Clear();

            _connection.Users.Remove(user);
            return _connection.SaveChanges() is 1;
        }

        public bool RemoveUserById(long id) => RemoveUserEntity(GetUserByIdEntity(id));

        public bool EditUser(UserDetailModel userDetailModel)
        {
            _connection.ChangeTracker.Clear();

            var entityUser = _connection.Users.First(x => x.ID == userDetailModel.ID);
            var mappedEntityUser = Mapper.MapDetailModelToEntity(userDetailModel);

            if (entityUser != mappedEntityUser)
            {
                _connection.Entry(entityUser).CurrentValues.SetValues(mappedEntityUser);

                return _connection.SaveChanges() is 1;
            }
            else
                return false;
        }

        public bool ChangePassword(UserDetailModel userDetailModel, string newPassword)
        {
            var (Salt, Hash) = Crypto.Encrypt(newPassword);

            userDetailModel.Hash = Hash;
            userDetailModel.Salt = Salt;

            return EditUser(userDetailModel);
        }

        public UserDetailModel GetUserByNick(string nick) => Mapper.MapEntityToDetailModel(_connection.Users.FirstOrDefault(x => x.NickName == nick) ?? new());

        public UserListModel GetUserListByNick(string nick) => Mapper.MapEntityToListModel(_connection.Users.First(x => x.NickName == nick) ?? new());

        public UserDetailModel GetUserByID(long id) => Mapper.MapEntityToDetailModel(GetUserByIdEntity(id));

        private User GetUserByIdEntity(long id) => _connection.Users.First(x => x.ID == id);

        public UserListModel GetUserListByID(long id) => Mapper.MapEntityToListModel(_connection.Users.First(x => x.ID == id));

        public async Task<ObservableCollection<UserDetailModel>> GetUsers() => await GetUsersCollection(UsersPredicate(), Mapper.MapEntityToDetailModel);

        public async Task<ObservableCollection<UserListModel>> GetUsersList() => await GetUsersCollection(UsersPredicate(), Mapper.MapEntityToListModel);

        private async Task<ObservableCollection<T>> GetUsersCollection<T>(Func<User, bool> filterPredicate, Func<User, T> mapEntityToModel) where T : UserListModel
        {
            return new(await Task.Run(() =>
            {
                var connection = _cloudCashDbContextFactory.CreateDbContext();

                return connection.Users.AsQueryable().Where(filterPredicate).Select(mapEntityToModel).ToList() ?? null;
            }));
        }

        private static Func<User, bool> UsersPredicate() => x => true;
    }
}
