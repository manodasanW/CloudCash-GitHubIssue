using System;
using System.ComponentModel.DataAnnotations;

namespace CloudCash.Common.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class DataClassAttribute : Attribute
    {
        [Required]
        public Type DataClassType { get; set; }

        public DataClassAttribute(Type dataClassType)
        {
            DataClassType = dataClassType;
        }
    }
}
