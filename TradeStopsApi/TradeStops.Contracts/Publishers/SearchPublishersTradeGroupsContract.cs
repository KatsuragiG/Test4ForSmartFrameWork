namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to search trade groups
    /// </summary>
    public class SearchPublishersTradeGroupsContract
    {
        /// <summary>
        /// Id of portfolio to which Trade groups belong
        /// </summary>
        public int PortfolioId { get; set; }
    }
}
