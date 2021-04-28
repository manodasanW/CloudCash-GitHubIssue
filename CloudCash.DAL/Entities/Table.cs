using CloudCash.DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace CloudCash.DAL.Entities
{
    public class Table : EntityBase
    {
        [Required]
        public DateTime StartDateTime { get; set; } = DateTime.Now;

        public DateTime EndDateTime { get; set; } = DateTime.MinValue;

        public ICollection<Sell> Sells { get; set; } = new ObservableCollection<Sell>();

        public ICollection<Payment> Payments { get; set; } = new ObservableCollection<Payment>();

        [Required]
        public TableInfo TableInfo { get; set; }
    }
}
