using CloudCash.BL.DTOs.Users;
using CloudCash.Common.Enums;
using CloudCash.Common.ModelBase;
using System;

namespace CloudCash.BL.DTOs.UserLog
{
    public record UserLogDetailModel : ListModelBase
    {
        public DateTime DateTime { get; set; }

        public UserListModel User { get; set; }

        public UserLogType LogType { get; set; }

        public override void CheckValues()
        {
            throw new NotImplementedException();
        }
    }
}
