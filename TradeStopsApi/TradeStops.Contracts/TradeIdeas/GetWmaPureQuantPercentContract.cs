using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Contract to get weighted moving average for the pure quant percent
    /// </summary>
    public class GetWmaPureQuantPercentContract
    {
        /// <summary>
        /// The type of market outlook
        /// </summary>
        public MarketOutlookTypes MarketOutlookId { get; set; }

        /// <summary>
        /// The date of pure quant indicator
        /// </summary>
        public DateTime IndicatorDate { get; set; }

        /// <summary>
        /// Pure quant percent
        /// </summary>
        public decimal PureQuantPercent { get; set; }
    }
}
