using CloudCash.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CloudCash.Common.Functions
{
    public static class RightsDecoder
    {
        public static List<Right> DecodeRights(this object rightToDecode) => DecodeRights(rightToDecode.CheckType<Right>());

        public static List<Right> DecodeRights(Right rightToDecode)
        {
            var allRights = Enum.GetValues(typeof(Right)).Cast<Right>().ToList();
            var result = new List<Right>();

            foreach (var right in allRights)
            {
                if ((right & rightToDecode) != 0)
                    result.Add(right);
            }

            return result;
        }

        public static Right EncodeRights(this object rightToDecode) => EncodeRights(rightToDecode.CheckType<List<Right>>());

        public static Right EncodeRights(List<Right> rights)
        {
            var result = Right.None;

            foreach (var right in rights)
            {
                result |= right;
            }

            return result;
        }
    }
}
