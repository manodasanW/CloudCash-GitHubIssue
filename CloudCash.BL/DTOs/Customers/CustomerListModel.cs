using CloudCash.Common.ModelBase;

namespace CloudCash.BL.DTOs.Customers
{
    public record CustomerListModel : ListModelBase
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public override void CheckValues()
        {
            throw new System.NotImplementedException();
        }
    }
}
