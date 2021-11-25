using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Information about chart item for the rank.
    /// </summary>
    public class GlobalRankAllocationContract
    {
        /// <summary>
        /// Global rank, calculated as an average rating across all criteria.
        /// </summary>
        public GlobalRankTypes GlobalRank { get; set; }

        /// <summary>
        /// Percentage of symbols in portfolio with rank.
        /// </summary>
        public decimal PercentageValue { get; set; }
    }
}
