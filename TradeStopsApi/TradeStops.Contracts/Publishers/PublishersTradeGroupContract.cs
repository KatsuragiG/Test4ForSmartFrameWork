namespace TradeStops.Contracts
{
    /// <summary>
    /// Trade group contract
    /// </summary>
    public class PublishersTradeGroupContract
    {
        /// <summary>
        /// Trade group Id
        /// </summary>
        public int TradeGroupId { get; set; }

        /// <summary>
        /// Portfolio Id
        /// </summary>
        public int PortfolioId { get; set; }

        /// <summary>
        /// Trade group value
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Position number of trade group on the grid
        /// </summary>
        public int Position { get; set; }
    }
}
