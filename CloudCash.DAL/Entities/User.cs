using CloudCash.Common.Enums;
using CloudCash.DAL.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace CloudCash.DAL.Entities
{
    public class User : EntityBase
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string NickName { get; set; }

        [Required]
        public byte[] Hash { get; set; }

        [Required]
        public byte[] Salt { get; set; }

        [Required]
        public Right Rights { get; set; }

        public Card Card { get; set; }
    }
}
