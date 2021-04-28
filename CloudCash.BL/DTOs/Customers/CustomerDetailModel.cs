using CloudCash.BL.DTOs.Cards;

namespace CloudCash.BL.DTOs.Customers
{
    public record CustomerDetailModel : CustomerListModel
    {
        public uint BonusPoints { get; set; }

        public string Email { get; set; }

        public CardListModel Card { get; set; } = new();
    }
}
