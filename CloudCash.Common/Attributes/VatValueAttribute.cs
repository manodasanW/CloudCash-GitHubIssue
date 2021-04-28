using System;
using System.ComponentModel.DataAnnotations;

namespace CloudCash.Common.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class VatValueAttribute : Attribute
    {
        [Required]
        public byte VatValue { get; set; }

        public VatValueAttribute(byte vatValue)
        {
            VatValue = vatValue;
        }
    }
}
