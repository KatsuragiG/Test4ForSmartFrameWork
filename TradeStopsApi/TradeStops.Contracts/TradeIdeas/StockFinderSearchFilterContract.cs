using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Filter
    /// </summary>
    public class StockFinderSearchFilterContract
    {
        /// <summary>
        /// Filter type
        /// </summary>
        public StockFinderFilterTypes FilterType { get; set; }

        /// <summary>
        /// Index number
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Filter params
        /// </summary>
        public StockFinderSearchFilterParamsContract Parameters { get; set; }
    }
}
