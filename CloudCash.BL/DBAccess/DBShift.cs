using CloudCash.BL.DbAccess.Base;
using CloudCash.BL.DTOs.Shifts;
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
    public class DbShift : DbBase
    {
        public DbShift(IDbContextFactory cloudCashDbContextFactory) : base(cloudCashDbContextFactory) { }

        public bool AddShift(ShiftDetailModel shifDetailModel)
        {
            _connection.ChangeTracker.Clear();

            var entity = Mapper.MapDetailModelToEntity(shifDetailModel);

            entity.User = _connection.Users.FirstOrDefault(x => x.ID == shifDetailModel.User.ID);

            _connection.Shifts.Add(entity);

            return _connection.SaveChanges() is 1;
        }

        public bool RemoveShift(ShiftDetailModel shifDetailModel)
        {
            _connection.ChangeTracker.Clear();

            var entity = Mapper.MapDetailModelToEntity(shifDetailModel);

            entity.User = _connection.Users.FirstOrDefault(x => x.ID == shifDetailModel.User.ID);

            _connection.Shifts.Remove(entity);

            return _connection.SaveChanges() is 1;
        }

        public bool EditShift(ShiftDetailModel shifDetailModel)
        {
            _connection.ChangeTracker.Clear();

            var entityShift = GetShiftWithIncludesData(_connection).First(x => x.ID == shifDetailModel.ID);
            var mappedEntityShift = Mapper.MapDetailModelToEntity(shifDetailModel);

            if (entityShift != mappedEntityShift)
            {
                _connection.Entry(entityShift).CurrentValues.SetValues(mappedEntityShift);

                return _connection.SaveChanges() is 1;
            }
            else
                return false;
        }

        public ShiftDetailModel GetShiftByID(uint id) => Mapper.MapEntityToDetailModel(GetShiftWithIncludesData(_connection).First(x => x.ID == id));

        public ShiftDetailModel GetLastShiftRecord() => Mapper.MapEntityToDetailModel(GetShiftWithIncludesData(_connection).OrderBy(x => x.ID).LastOrDefault() ?? new());

        public async Task<ObservableCollection<ShiftDetailModel>> GetShifts() => await GetShiftsCollection(ShiftsPredicate(), Mapper.MapEntityToDetailModel);

        public async Task<ObservableCollection<ShiftListModel>> GetShiftsList() => await GetShiftsCollection(ShiftsPredicate(), Mapper.MapEntityToListModel);

        private async Task<ObservableCollection<T>> GetShiftsCollection<T>(Func<Shift, bool> filterPredicate, Func<Shift, T> mapEntityToModel) where T : ShiftListModel
        {
            return new(await Task.Run(() =>
            {
                using var connection = _cloudCashDbContextFactory.CreateDbContext();

                return GetShiftWithIncludesData(connection).AsQueryable()?.Where(filterPredicate).Select(mapEntityToModel) ?? null;
            }));
        }

        private static Func<Shift, bool> ShiftsPredicate() => x => true;

        private IIncludableQueryable<Shift, User> GetShiftWithIncludesData(CloudCashDbContext connection) => connection.Shifts.Include(x => x.User);
    }
}
