using CloudCash.Common.Enums;
using CloudCash.DAL.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace CloudCash.DAL.Entities
{
    public class UserLog : EntityBase
    {
        [Required]
        public UserLogType LogType { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        public User User { get; set; }
    }
}
