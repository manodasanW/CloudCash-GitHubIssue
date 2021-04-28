using CloudCash.BL.DTOs.TableInfo;
using CloudCash.Common.ModelBase;
using System;

namespace CloudCash.BL.DTOs.Tables
{
    public record TableListModel : ListModelBase
    {
        public DateTime StartDateTime { get; set; } = DateTime.Now;

        public TableInfoDetailModel TableInfo { get; set; } = new();

        public override void CheckValues()
        {
            throw new NotImplementedException();
        }
    }
}
