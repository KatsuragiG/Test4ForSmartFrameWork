using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// The contract contains intraday ticks data aggregated by 1 min.
    /// </summary>
    public class IntradayPricesAggregatedDataContract
    {
        /// <summary>
        /// Symbol Id.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Transaction time in UTC format.
        /// </summary>
        public DateTime TransactionTime { get; set; }

        /// <summary>
        /// Open price.
        /// </summary>
        public double Open { get; set; }

        /// <summary>
        /// Highest price.
        /// </summary>
        public double High { get; set; }

        /// <summary>
        /// Lowest price.
        /// </summary>
        public double Low { get; set; }

        /// <summary>
        /// Close price.
        /// </summary>
        public double Close { get; set; }

        /// <summary>
        /// Number of transaction elements.
        /// </summary>
        public double Volume { get; set; }

        /// <summary>
        /// Intraday price type based on exchange working hours. 
        /// </summary>
        public IntradayPriceTypes PriceType { get; set; }
    }
}
