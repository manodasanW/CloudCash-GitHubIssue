using CloudCash.Common.Enums;
using CloudCash.DAL.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace CloudCash.DAL.Entities
{
    public class Shift : EntityBase
    {
        [Required]
        public DateTime DateTime { get; set; } = DateTime.Now;

        public User User{ get; set; }

        [Required]
        public uint CashValue { get; set; } = 0;

        [Required]
        public ShiftRecordType ShiftRecordType { get; set; }

        // todo: Do zareni smeny potrebujeme:
        // - seznam prodeju
        // - seznam plateb
        // - slevy
        // - prijmy a vydaje
        // - otevrene ucty s hodnotou
    }
}
