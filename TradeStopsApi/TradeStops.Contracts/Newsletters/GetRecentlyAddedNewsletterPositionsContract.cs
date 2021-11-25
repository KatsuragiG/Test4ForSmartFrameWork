namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to get recently added newsletter positions
    /// </summary>
    public class GetRecentlyAddedNewsletterPositionsContract
    {
        /// <summary>
        /// Maximum number of Newsletter Positions to return
        /// </summary>
        public int MaximumNumberOfPositions { get; set; }

        /// <summary>
        /// Determines if result should contain stocks or/and crypto:
        /// if true - only crypto,
        /// if false - only stocks,
        /// if null - everything (no filtering).
        /// </summary>
        public bool? IsCrypto { get; set; }
    }
}
