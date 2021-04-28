using CloudCash.DAL.Entities.Base;

namespace CloudCash.DAL.Entities
{
    public class ProductCategory : CategoryBase
    {
        public bool PrintSeparately { get; set; }
    }
}
