namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters for creating a trade group
    /// </summary>
    public class CreatePublishersTradeGroupContract
    {
        /// <summary>
        /// Portfolio Id
        /// </summary>
        public int PortfolioId { get; set; }

        /// <summary>
        /// Trade group value
        /// </summary>
        public string Name { get; set; }
    }
}
