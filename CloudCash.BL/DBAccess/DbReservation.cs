using CloudCash.BL.DTOs.Reservations;
using CloudCash.BL.DTOs.Tables;
using CloudCash.DAL.Entities;
using CloudCash.DAL.Factories;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace CloudCash.BL.DbAccess
{
    public class DbReservation
    {
        private readonly IDbContextFactory _cloudCashDbContextFactory;

        public DbReservation(IDbContextFactory cloudCashDbContextFactory)
        {
            _cloudCashDbContextFactory = cloudCashDbContextFactory;
        }

        public bool AddCustomer(ReservationDetailModel reservationDetailModel)
        {
            using var connection = _cloudCashDbContextFactory.CreateDbContext();

            connection.Reservations.Add(Mapper.MapDetailModelToEntity(reservationDetailModel));
            return connection.SaveChanges() is 1;
        }

        public bool RemoveCustomer(ReservationDetailModel reservationDetailModel)
        {
            using var connection = _cloudCashDbContextFactory.CreateDbContext();
            connection.Reservations.Remove(Mapper.MapDetailModelToEntity(reservationDetailModel));
            return connection.SaveChanges() is 1;
        }

        public bool EditCustomer(ReservationDetailModel reservationDetailModel)
        {
            using var connection = _cloudCashDbContextFactory.CreateDbContext();

            var entityReservation = connection.Reservations.First(x => x.ID == reservationDetailModel.ID);
            var mappedEntityReservation = Mapper.MapEntityToDetailModel(entityReservation);

            if (mappedEntityReservation != reservationDetailModel)
            {
                entityReservation = Mapper.MapDetailModelToEntity(reservationDetailModel);
                return connection.SaveChanges() is 1;
            }
            else
                return false;
        }

        public ObservableCollection<ReservationDetailModel> GetReservationsByTableInfo(TableListModel tableListModel) => GetReservationsCollection(ReservationsByTableInfoPredicate(tableListModel), Mapper.MapEntityToDetailModel);

        public ObservableCollection<ReservationListModel> GetReservationsByTableInfoList(TableListModel tableListModel) => GetReservationsCollection(ReservationsByTableInfoPredicate(tableListModel), Mapper.MapEntityToListModel);

        public ObservableCollection<ReservationDetailModel> GetReservationsByName(string name) => GetReservationsCollection(ReservationsByNamePredicate(name), Mapper.MapEntityToDetailModel);

        public ObservableCollection<ReservationListModel> GetReservationsByNameList(string name) => GetReservationsCollection(ReservationsByNamePredicate(name), Mapper.MapEntityToListModel);

        public ObservableCollection<ReservationDetailModel> GetReservationsByEmail(string email) => GetReservationsCollection(ReservationsByEmailPredicate(email), Mapper.MapEntityToDetailModel);

        public ObservableCollection<ReservationListModel> GetReservationsByEmailList(string email) => GetReservationsCollection(ReservationsByEmailPredicate(email), Mapper.MapEntityToListModel);

        public ObservableCollection<ReservationDetailModel> GetReservationsByStartTime(DateTime startTime) => GetReservationsCollection(ReservationsByStartTimePredicate(startTime), Mapper.MapEntityToDetailModel);

        public ObservableCollection<ReservationListModel> GetReservationsByStartTimeList(DateTime startTime) => GetReservationsCollection(ReservationsByStartTimePredicate(startTime), Mapper.MapEntityToListModel);

        public ObservableCollection<ReservationDetailModel> GetReservations() => GetReservationsCollection(ReservationsPredicate(), Mapper.MapEntityToDetailModel);

        public ObservableCollection<ReservationListModel> GetReservationsList() => GetReservationsCollection(ReservationsPredicate(), Mapper.MapEntityToListModel);

        private ObservableCollection<T> GetReservationsCollection<T>(Func<Reservation, bool> filterPredicate, Func<Reservation, T> mapEntityToModel) where T : ReservationListModel
        {
            using var connection = _cloudCashDbContextFactory.CreateDbContext();

            return new ObservableCollection<T>(connection.Reservations.AsQueryable().Where(filterPredicate).Select(mapEntityToModel));
        }

        private static Func<Reservation, bool> ReservationsPredicate() => x => true;

        private static Func<Reservation, bool> ReservationsByTableInfoPredicate(TableListModel tableListModel) => x => x.SelectedTable.ID == tableListModel.ID;

        private static Func<Reservation, bool> ReservationsByNamePredicate(string name) => x => x.Name == name;

        private static Func<Reservation, bool> ReservationsByEmailPredicate(string email) => x => x.Email == email;

        private static Func<Reservation, bool> ReservationsByStartTimePredicate(DateTime startTime) => x => x.StartTime == startTime;
    }
}
