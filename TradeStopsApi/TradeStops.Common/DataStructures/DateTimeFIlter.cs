using System;

namespace TradeStops.Common.DataStructures
{
    public class DateTimeFilter
    {
        /// <summary>
        /// Minimum value
        /// </summary>
        public DateTime? MinValue { get; set; }

        /// <summary>
        /// Maximum value
        /// </summary>
        public DateTime? MaxValue { get; set; }
    }
}
