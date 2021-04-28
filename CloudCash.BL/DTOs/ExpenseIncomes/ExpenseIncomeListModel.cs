using CloudCash.Common.Enums;
using CloudCash.Common.ModelBase;
using System;

namespace CloudCash.BL.DTOs.ExpenseIncomes
{
    public record ExpenseIncomeListModel : ListModelBase
    {
        public DateTime DateTime { get; set; } = DateTime.Now;

        public uint Price { get; set; }

        public EIType EIType { get; set; }

        public override void CheckValues()
        {
            throw new System.NotImplementedException();
        }
    }
}
