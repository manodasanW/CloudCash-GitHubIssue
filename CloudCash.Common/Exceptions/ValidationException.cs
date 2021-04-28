using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CloudCash.Common.Exceptions
{
    public class ValidationException : Exception
    {
        public string PropertyName { get; set; } = string.Empty;
        public string ValidationErrorMessage { get; set; } = string.Empty;

        public ValidationException(string propertyName)
        {
            PropertyName = propertyName;
        }

        public ValidationException(string propertyName, string message) : base(message)
        {
            PropertyName = propertyName;
            ValidationErrorMessage = message;
        }

        public ValidationException(string propertyName, string message, Exception innerException) : base(message, innerException)
        {
            PropertyName = propertyName;
            ValidationErrorMessage = message;
        }

        protected ValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
