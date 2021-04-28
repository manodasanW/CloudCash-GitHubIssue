using System;

namespace CloudCash.Common.Functions
{
    public static class CheckerExtension
    {
        public static void CheckNull(this object obj, string paramName = "")
        {
            if (obj is null)
                throw new ArgumentNullException(paramName);
        }

        public static T CheckType<T>(this object obj, bool silentCheck = false, string paramName = "")
        {
            obj.CheckNull(paramName);

            if (silentCheck && obj is not T)
                return default;

            return (T)obj;
        }
    }
}
