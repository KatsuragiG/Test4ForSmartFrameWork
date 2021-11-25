namespace TradeStops.Contracts
{
    /// <summary>
    /// Likelihood of stock dropping fact parameters.
    /// </summary>
    public class LikelihoodDropFactContract
    {
        /// <summary>
        /// Number of days of consecutive price change.
        /// </summary>
        public int Period { get; set; }

        /// <summary>
        /// The price has changed times.
        /// </summary>
        public int CrossLine { get; set; }

        /// <summary>
        /// Likelihood of change percent.
        /// </summary>
        public decimal LikelihoodPercent { get; set; }

        /// <summary>
        /// Lower or upper band percent.
        /// </summary>
        public decimal BandPercentage { get; set; }
    }
}
