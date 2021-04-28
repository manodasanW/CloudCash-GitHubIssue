using System;
using System.ComponentModel.DataAnnotations;

namespace CloudCash.Common.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class ValidityAttribute : Attribute
    {
        [Required]
        public bool IsValid { get; set; }

        public ValidityAttribute(bool isValid)
        {
            IsValid = isValid;
        }
    }
}
