using CloudCash.BL.DTOs.Tables;
using CloudCash.Common.ModelBase;
using System;

namespace CloudCash.BL.DTOs.Payments
{
    public record PaymentListModel : ListModelBase
    {
        public DateTime DateTime { get; set; } = DateTime.Now;

        public bool IsPartial { get; set; }

        public uint Price { get; set; }

        public TableListModel Table { get; set; } = new();

        public override void CheckValues()
        {
            throw new System.NotImplementedException();
        }
    }
}
