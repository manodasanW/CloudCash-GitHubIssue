using CloudCash.Common.Enums;
using CloudCash.DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace CloudCash.DAL.Entities
{
    public class Payment : EntityBase
    {
        [Required]
        public DateTime DateTime { get; set; } = DateTime.Now;

        public bool IsPartial { get; set; }

        public Table Table { get; set; }

        public uint Price { get; set; }

        [Required]
        public ICollection<Sell> Sells { get; set; } = new ObservableCollection<Sell>();

        public Customer Customer { get; set; }

        [Required]
        public PaymentType PaymentType { get; set; }

        public string Note { get; set; } = string.Empty;

        public byte Discount { get; set; }
    }
}
