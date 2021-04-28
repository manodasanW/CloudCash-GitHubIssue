using CloudCash.DAL.Entities.Base;
using System.ComponentModel.DataAnnotations;
using CloudCash.Common.Enums;

namespace CloudCash.DAL.Entities
{
    public class Product : EntityBase
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public ProductCategory Category { get; set; }

        public VatLevel VatLevel { get; set; }
    }
}
