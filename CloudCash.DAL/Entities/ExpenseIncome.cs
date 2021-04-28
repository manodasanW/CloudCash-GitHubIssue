using CloudCash.Common.Enums;
using CloudCash.DAL.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace CloudCash.DAL.Entities
{
    public class ExpenseIncome : EntityBase
    {
        [Required]
        public DateTime DateTime { get; set; } = DateTime.Now;

        public User User { get; set; }

        [Required]
        public uint Price { get; set; }

        public EIType EIType { get; set; } = EIType.Income;
    }
}
