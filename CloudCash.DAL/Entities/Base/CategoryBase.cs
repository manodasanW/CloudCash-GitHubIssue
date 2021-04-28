using System.ComponentModel.DataAnnotations;

namespace CloudCash.DAL.Entities.Base
{
    public class CategoryBase : EntityBase
    {
        [Required]
        public string Name { get; set; }
    }
}
