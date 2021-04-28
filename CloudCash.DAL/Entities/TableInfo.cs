using CloudCash.DAL.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace CloudCash.DAL.Entities
{
    public class TableInfo : EntityBase
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public byte Size { get; set; } = 4;

        [Required]
        public short PositionX { get; set; }

        [Required]
        public short PositionY { get; set; }

        [Required]
        public TableCategory Category { get; set; }
    }
}
