using CloudCash.DAL.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace CloudCash.DAL.Entities
{
    public class Card : EntityBase
    {
        [Required]
        public int Number { get; set; }
    }
}
