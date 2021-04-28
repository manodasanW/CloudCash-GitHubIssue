using CloudCash.DAL.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace CloudCash.DAL.Entities
{
    public class Customer : EntityBase
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public uint BonusPoints { get; set; } = 0;

        [Required]
        public string Email { get; set; }

        public Card Card { get; set; }
    }
}
