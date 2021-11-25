namespace TradeStops.Contracts
{
    /// <summary>
    /// Contract with percentage values for each Global rank type
    /// </summary>
    public class GlobalRankDistributionContract
    {
        /// <summary>
        /// Strong bearish percent.
        /// </summary>
        public decimal StrongBearishPercent { get; set; }

        /// <summary>
        /// Bearish percent.
        /// </summary>
        public decimal BearishPercent { get; set; }

        /// <summary>
        /// Neutral percent.
        /// </summary>
        public decimal NeutralPercent { get; set; }

        /// <summary>
        /// Bullish percent.
        /// </summary>
        public decimal BullishPercent { get; set; }

        /// <summary>
        /// Strong bullish percent.
        /// </summary>
        public decimal StrongBullishPercent { get; set; }
    }
}
