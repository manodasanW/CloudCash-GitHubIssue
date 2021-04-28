using CloudCash.DAL.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace CloudCash.DAL.Entities
{
    public class Reservation : EntityBase
    {
        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        public byte PeopleCount { get; set; }

        public TableInfo SelectedTable { get; set; }
    }
}
