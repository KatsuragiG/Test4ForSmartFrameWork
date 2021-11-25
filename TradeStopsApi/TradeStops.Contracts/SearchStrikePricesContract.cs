using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to find options strike prices
    /// </summary>
    public class SearchStrikePricesContract
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
        /// Expiration date
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// Property to identify whether it’s necessary to return delisted/not delisted only.
        /// </summary>
        public bool Delisted { get; set; }
    }
}