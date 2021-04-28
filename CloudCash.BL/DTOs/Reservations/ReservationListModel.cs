using CloudCash.Common.ModelBase;
using System;

namespace CloudCash.BL.DTOs.Reservations
{
    public record ReservationListModel : ListModelBase
    {
        public DateTime StartTime { get; set; } = DateTime.Now;

        public DateTime EndTime { get; set; } = DateTime.MinValue;

        public string Name { get; set; }

        public override void CheckValues()
        {
            throw new System.NotImplementedException();
        }
    }
}
