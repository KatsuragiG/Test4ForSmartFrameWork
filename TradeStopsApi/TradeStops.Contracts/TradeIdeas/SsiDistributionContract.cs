namespace TradeStops.Contracts
{
    /// <summary>
    /// Contract with percentage values for each SSI status
    /// </summary>
    public class SsiDistributionContract
    {
        /// <summary>
        /// Green SSI percent
        /// </summary>
        public decimal GreenSsiPercent { get; set; }

        /// <summary>
        /// Yellow SSI up-trend percent
        /// </summary>
        public decimal YellowSsiUpTrendPercent { get; set; }

        /// <summary>
        /// Yellow SSI side-trend percent
        /// </summary>
        public decimal YellowSsiSideTrendPercent { get; set; }

        /// <summary>
        /// Yellow SSI down-trend percent
        /// </summary>
        public decimal YellowSsiDownTrendPercent { get; set; }

        /// <summary>
        /// Red SSI percent
        /// </summary>
        public decimal RedSsiPercent { get; set; }

        /// <summary>
        /// Undefined SSI percent
        /// </summary>
        public decimal UndefinedSsiPercent { get; set; }
    }
}