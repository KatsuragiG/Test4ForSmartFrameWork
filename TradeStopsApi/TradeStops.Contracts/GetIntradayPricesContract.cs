using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters for getting Intraday Prices
    /// </summary>
    public class GetIntradayPricesContract
    {
        /// <summary>
        /// Symbol ID
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// (optional) From Date
        /// </summary>
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// To Date
        /// </summary>
        public DateTime ToDate { get; set; }

        /// <summary>
        /// Limit on the number of points
        /// </summary>
        public int MaxPricePoints { get; set; }

        /// <summary>
        /// (optional) Indicates whether resulting data set must be ordered by TradeDate in ascending or descending order.
        /// </summary>
        public OrderTypes? OrderType { get; set; }
    }
}