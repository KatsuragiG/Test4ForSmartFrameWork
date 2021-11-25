using System;
using System.Linq;

namespace TradeStops.Common.Utils
{
    public static class TypeUtils
    {
        public static string GetTypeName(Type t, string openingBrace = "<", string closingBrace = ">")
        {
            if (!t.IsGenericType)
            {
                return t.Name;
            }
                
            string genericTypeName = t.GetGenericTypeDefinition().Name;

            genericTypeName = genericTypeName.Substring(0, genericTypeName.IndexOf('`'));

            var genericArgsArray = t.GetGenericArguments().Select(ta => GetTypeName(ta, openingBrace, closingBrace)).ToArray();

            string genericArgs = string.Join(", ", genericArgsArray);

            return genericTypeName + openingBrace + genericArgs + closingBrace;
        }
    }
}
