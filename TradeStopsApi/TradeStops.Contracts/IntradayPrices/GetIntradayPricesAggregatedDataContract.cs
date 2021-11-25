using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// The contract for receiving intraday ticks data aggregated by 1 min.
    /// </summary>
    public class GetIntradayPricesAggregatedDataContract
    {
        /// <summary>
        /// Symbol Id.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Defines the start date of the range.
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Defines the end date of the range.
        /// </summary>
        public DateTime ToDate { get; set; }
    }
}
