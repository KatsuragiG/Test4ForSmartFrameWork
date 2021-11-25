using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to find options expiration dates
    /// </summary>
    public class SearchExpirationDatesContract
    {
        /// <summary>
        /// Underline stock ID.
        /// </summary>
        public int ParentSymbolId { get; set; }

        /// <summary>
        /// Option type.
        /// </summary>
        public OptionTypes? Type { get; set; }

        /// <summary>
        /// Start date
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Finish date
        /// </summary>
        public DateTime ToDate { get; set; }

        /// <summary>
        /// Property to identify whether it’s necessary to return delisted/not delisted only or all expiration dates.
        /// </summary>
        public bool? Delisted { get; set; }
    }
}