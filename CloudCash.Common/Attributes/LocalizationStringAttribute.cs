using System;
using System.ComponentModel.DataAnnotations;

namespace CloudCash.Common.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class LocalizationStringAttribute : Attribute
    {
        [Required]
        public string StringValue { get; set; }

        public LocalizationStringAttribute(string stringValue)
        {
            StringValue = stringValue;
        }
    }
}
