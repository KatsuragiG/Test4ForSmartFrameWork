namespace TradeStops.Contracts
{
    /// <summary>
    /// Contract with all current distributions of symbol group.
    /// </summary>
    public class SymbolGroupDistributionsContract
    {
        /// <summary>
        /// Percentage values for each SSI status.
        /// </summary>
        public SsiDistributionContract SsiDistribution { get; set; }

        /// <summary>
        /// Percentage values for each Trend type.
        /// </summary>
        public TrendDistributionContract TrendDistribution { get; set; }

        /// <summary>
        /// Percentage values for each Global rank type.
        /// </summary>
        public GlobalRankDistributionContract GlobalRankDistribution { get; set; }
    }
}
