using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Option data contract, including option statistics.
    /// </summary>
    public class OptionDataContract
    {
        /// <summary>
        /// Option symbol ID.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Underline stock ID.
        /// </summary>
        public int ParentSymbolId { get; set; }

        /// <summary>
        /// Symbol's ticker, like 'AAPL'.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Option strike price.
        /// </summary>
        public decimal StrikePrice { get; set; }

        /// <summary>
        /// Option contract size.
        /// </summary>
        public int? ContractSize { get; set; }

        /// <summary>
        /// Days before expiration.
        /// </summary>
        public int DaysBeforeExpiration { get; set; }

        /// <summary>
        /// Option type.
        /// </summary>
        public OptionTypes OptionType { get; set; }

        /// <summary>
        /// Option latest close price.
        /// </summary>
        public decimal? LatestClose { get; set; }

        /// <summary>
        /// Latest option volume value.
        /// </summary>
        public decimal? Volume { get; set; }

        /// <summary>
        /// Latest close price of underlying stock.
        /// </summary>
        public decimal? ParentLatestClose { get; set; }

        /// <summary>
        /// The minimum price a seller is willing to accept.
        /// </summary>
        public decimal? Ask { get; set; }

        /// <summary>
        /// The maximum price a buyer is willing to pay.
        /// </summary>
        public decimal? Bid { get; set; }

        /// <summary>
        /// Bid/ask ratio.
        /// </summary>
        public decimal? BidAskRatio { get; set; }

        /// <summary>
        /// Moneyness value.
        /// </summary>
        public decimal? Moneyness { get; set; }

        /// <summary>
        /// Implied volatility value.
        /// </summary>
        public decimal? ImpliedVolatility { get; set; }

        /// <summary>
        /// Historical volatility value.
        /// </summary>
        public decimal? HistoricalVolatility { get; set; }

        /// <summary>
        /// Implied/historical volatility ratio.
        /// </summary>
        public decimal? VolatilityRatio { get; set; }

        /// <summary>
        /// Delta value.
        /// </summary>
        public decimal? Delta { get; set; }

        /// <summary>
        /// Gamma value.
        /// </summary>
        public decimal? Gamma { get; set; }

        /// <summary>
        /// Theta value.
        /// </summary>
        public decimal? Theta { get; set; }

        /// <summary>
        /// Vega value.
        /// </summary>
        public decimal? Vega { get; set; }

        /// <summary>
        /// Rho value.
        /// </summary>
        public decimal? Rho { get; set; }

        /// <summary>
        /// OptionPriceTypes value.
        /// </summary>
        public OptionPriceTypes OptionPriceType { get; set; }

        /// <summary>
        /// DecayType value.
        /// </summary>
        public DecayTypes DecayType { get; set; }

        /// <summary>
        /// Implied volatility percentile.
        /// </summary>
        public decimal? ImpliedVolatilityPercentile { get; set; }

        /// <summary>
        /// Implied volatility rank.
        /// </summary>
        public decimal? ImpliedVolatilityRank { get; set; }

        /// <summary>
        /// Option expiration date.
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// The number of options that traders and investors hold in active positions.
        /// </summary>
        public decimal? OpenInterest { get; set; }

        /// <summary>
        /// Option latest intraday price.
        /// </summary>
        public decimal? LatestPrice { get; set; }

        /// <summary>
        /// One day implied volatility rank change value
        /// </summary>
        public decimal? OneDayImpliedVolatilityRankChange { get; set; }

        /// <summary>
        /// One week implied volatility rank change value
        /// </summary>
        public decimal? OneWeekImpliedVolatilityRankChange { get; set; }
    }
}
