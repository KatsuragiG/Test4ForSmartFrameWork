// This extension method may be useful in the future,
// but currently we don't use attributtes to convert enums to strings

////using System;

////namespace TradeStops.Common.Extensions
////{
////    public static class EnumExtensions
////    {
////        public static string ToName(this Enum value)
////        {
////            var attribute = value.GetAttribute<DescriptionAttribute>();
////            return attribute == null ? value.ToString() : attribute.Description;
////        }

////        private static T GetAttribute<T>(this Enum value) where T : Attribute
////        {
////            var type = value.GetType();
////            var memberInfo = type.GetMember(value.ToString());
////            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
////            return (T)attributes[0];
////        }
////    }
////}
