using CloudCash.DAL.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace CloudCash.DAL.Entities
{
    public class Sell : EntityBase
    {
        [Required]
        public Table Table { get; set; }

        public Product Product { get; set; }

        [Required]
        public DateTime DateTime { get; set; } = DateTime.Now;

        public byte Discount { get; set; }

        public Payment Payment { get; set; }
    }
}
