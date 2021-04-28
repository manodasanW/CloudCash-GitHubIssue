using CloudCash.BL.DTOs.Customers;
using CloudCash.Common.ModelBase;

namespace CloudCash.BL.DTOs.Cards
{
    public record CardListModel : ListModelBase
    {
        public CustomerListModel Customer { get; set; }

        public int Number { get; set; }

        public override void CheckValues()
        {
            throw new System.NotImplementedException();
        }
    }
}
