using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Detailed quote and price information
    /// </summary>
    public class OptionChainContract
    {
        /// <summary>
        /// Option symbol ID.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Symbol's ticker, like 'AAPL'.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Option type.
        /// </summary>
        public OptionTypes Type { get; set; }

        /// <summary>
        /// Option strike price.
        /// </summary>
        public decimal StrikePrice { get; set; }

        /// <summary>
        /// The number of options that buyers and sellers exchange.
        /// </summary>
        public decimal? TradeVolume { get; set; }

        /// <summary>
        /// The minimum price a seller is willing to accept.
        /// </summary>
        public decimal? AskPrice { get; set; }

        /// <summary>
        /// The maximum price a buyer is willing to pay.
        /// </summary>
        public decimal? BidPrice { get; set; }

        /// <summary>
        /// The number of options that traders and investors hold in active positions.
        /// </summary>
        public decimal? OpenInterest { get; set; }

        /// <summary>
        /// Closing price for the given Trade Date.
        /// </summary>
        public decimal TradeClose { get; set; }

        /// <summary>
        /// ID of the currency.
        /// </summary>
        public int CurrencyId { get; set; }

        /// <summary>
        /// Currency symbol, like '$'.
        /// </summary>
        public string CurrencySign { get; set; }

        /// <summary>
        /// Daily gain for option.
        /// </summary>
        public decimal? DailyGain { get; set; }

        /// <summary>
        /// Daily gain percentage.
        /// </summary>
        public decimal? DailyGainPercentage { get; set; }
    }
}
