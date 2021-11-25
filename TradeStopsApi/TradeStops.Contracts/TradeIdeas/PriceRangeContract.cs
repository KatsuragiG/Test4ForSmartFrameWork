namespace TradeStops.Contracts
{
    /// <summary>
    /// Price range
    /// </summary>
    public class PriceRangeContract
    {
        /// <summary>
        /// Lowest price for the specified period
        /// </summary>
        public PriceContract MinPrice { get; set; }

        /// <summary>
        /// Highest price for the specified period
        /// </summary>
        public PriceContract MaxPrice { get; set; }

        /// <summary>
        /// Current (Latest) price
        /// </summary>
        public PriceContract CurrentPrice { get; set; }
    }
}
