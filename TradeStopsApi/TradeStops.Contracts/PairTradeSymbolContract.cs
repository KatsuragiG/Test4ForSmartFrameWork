namespace TradeStops.Contracts
{
    /// <summary>
    /// PairTrade symbol information
    /// </summary>
    public class PairTradeSymbolContract
    {
        /// <summary>
        /// Composite PairTrade ticker, like 'AAPL/MSFT'
        /// </summary>
        public string Ticker { get; set; }

        /// <summary>
        /// Composite PairTrade name, like 'Apple corp/Microsoft corp'
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Exchange name, like 'TSX Venture/NYSE'
        /// </summary>
        public string ExchangeName { get; set; }
    }
}