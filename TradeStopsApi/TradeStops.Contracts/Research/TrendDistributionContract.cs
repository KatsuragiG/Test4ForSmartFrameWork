namespace TradeStops.Contracts
{
    /// <summary>
    /// Contract with percentage values for each Trend types.
    /// </summary>
    public class TrendDistributionContract
    {
        /// <summary>
        /// Up trend percent.
        /// </summary>
        public decimal UpTrendPercent { get; set; }

        /// <summary>
        /// Down trend percent.
        /// </summary>
        public decimal DownTrendPercent { get; set; }

        /// <summary>
        /// Side trend percent.
        /// </summary>
        public decimal SideTrendPercent { get; set; }
    }
}
