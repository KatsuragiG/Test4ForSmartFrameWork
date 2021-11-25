using System;
using System.Collections.Generic;
using System.Linq;

namespace TradeStops.Common.Utils
{
    public static class EnumUtils
    {
        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static T Parse<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static bool IsDefined<T>(T value)
        {
            return Enum.IsDefined(typeof(T), value);
        }
    }
}
