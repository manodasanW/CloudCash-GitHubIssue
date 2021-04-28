using CloudCash.BL.DTOs.Cards;
using CloudCash.BL.DTOs.Users;
using CloudCash.DAL.Entities;
using CloudCash.DAL.Factories;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace CloudCash.BL.DbAccess
{
    public class DbCard
    {
        private readonly IDbContextFactory _cloudCashDbContextFactory;

        public DbCard(IDbContextFactory cloudCashDbContextFactory)
        {
            _cloudCashDbContextFactory = cloudCashDbContextFactory;
        }

        public bool AddCustomer(CardDetailModel cardDetailModel)
        {
            using var connection = _cloudCashDbContextFactory.CreateDbContext();

            connection.Cards.Add(Mapper.MapDetailModelToEntity(cardDetailModel));
            return connection.SaveChanges() is 1;
        }

        public bool RemoveCustomer(CardDetailModel cardDetailModel)
        {
            using var connection = _cloudCashDbContextFactory.CreateDbContext();
            connection.Cards.Remove(Mapper.MapDetailModelToEntity(cardDetailModel));
            return connection.SaveChanges() is 1;
        }

        public bool EditCustomer(CardDetailModel cardDetailModel)
        {
            using var connection = _cloudCashDbContextFactory.CreateDbContext();

            var entityCard = connection.Cards.First(x => x.ID == cardDetailModel.ID);
            var mappedEntityCard = Mapper.MapEntityToDetailModel(entityCard);

            if (mappedEntityCard != cardDetailModel)
            {
                entityCard = Mapper.MapDetailModelToEntity(cardDetailModel);
                return connection.SaveChanges() is 1;
            }
            else
                return false;
        }

        public CardDetailModel GetCardByCustomer(UserDetailModel userDetailModel)
        {
            using var connection = _cloudCashDbContextFactory.CreateDbContext();

            //var card = connection.Cards.FirstOrDefault(x => x.Customer.ID == userDetailModel.ID);
            //return card is null ? null : Mapper.MapEntityToDetailModel(card);
            return null;
        }

        public CardDetailModel GetCardByNumber(int cardNumber)
        {
            using var connection = _cloudCashDbContextFactory.CreateDbContext();

            var card = connection.Cards.FirstOrDefault(x => x.Number == cardNumber);
            return card is null ? null : Mapper.MapEntityToDetailModel(card);
        }

        public ObservableCollection<CardDetailModel> GetCustomers() => GetCardsCollection(CardsPredicate(), Mapper.MapEntityToDetailModel);

        public ObservableCollection<CardListModel> GetCustomersList() => GetCardsCollection(CardsPredicate(), Mapper.MapEntityToListModel);

        private ObservableCollection<T> GetCardsCollection<T>(Func<Card, bool> filterPredicate, Func<Card, T> mapEntityToModel) where T : CardListModel
        {
            using var connection = _cloudCashDbContextFactory.CreateDbContext();

            return new ObservableCollection<T>(connection.Cards.AsQueryable().Where(filterPredicate).Select(mapEntityToModel));
        }

        private static Func<Card, bool> CardsPredicate() => x => true;
    }
}
