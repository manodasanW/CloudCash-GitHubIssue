using CloudCash.Common.Enums;
using CloudCash.Common.ModelBase;
using System;

namespace CloudCash.BL.DTOs.Shifts
{
    public record ShiftListModel : ListModelBase
    {
        public ShiftRecordType ShiftRecordType { get; set; }

        public DateTime DateTime { get; set; } = DateTime.Now;

        public override void CheckValues()
        {
            throw new System.NotImplementedException();
        }
    }
}
