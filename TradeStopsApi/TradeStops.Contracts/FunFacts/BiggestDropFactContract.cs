namespace TradeStops.Contracts
{
    /// <summary>
    /// Biggest drop fact parameters.
    /// </summary>
    public class BiggestDropFactContract
    {
        /// <summary>
        /// Fall period in days.
        /// </summary>
        public int Period { get; set; }

        /// <summary>
        /// Lower band of fall in percentage.
        /// </summary>
        public decimal LowerBandPercent { get; set; }
    }
}
