using System;
using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Stock Finder result
    /// </summary>
    public class OptionsScreenerResultContract
    {
        /// <summary>
        /// Symbol ID
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Ticker
        /// </summary>
        public SymbolContract Symbol { get; set; }

        /// <summary>
        /// List of strategies to apply as filter.
        /// </summary>
        public InvestmentStrategyTypes? Strategy { get; set; }

        /// <summary>
        /// Trade Type
        /// </summary>
        public TradeTypes TradeType { get; set; }

        /// <summary>
        /// Option operation type, which is a combination of the trade type and option type filters.
        /// </summary>
        public OptionOperationTypes OptionOperationType { get; set; }

        /// <summary>
        /// Moneyness value.
        /// </summary>
        public decimal? Moneyness { get; set; }

        /// <summary>
        /// Probability of profit value.
        /// </summary>
        public decimal? ProbabilityOfProfit { get; set; }

        /// <summary>
        /// Position size value.
        /// </summary>
        public decimal? PositionSize { get; set; }

        /// <summary>
        /// Option latest close price.
        /// </summary>
        public decimal LatestClosePrice { get; set; }

        /// <summary>
        /// Latest close price of underlying stock.
        /// </summary>
        public decimal UnderlyingClosePrice { get; set; }

        /// <summary>
        /// ROI value.
        /// </summary>
        public decimal?  Roi { get; set; }

        /// <summary>
        /// Maximum expected profit.
        /// </summary>
        public decimal? MaxProfit { get; set; }

        /// <summary>
        /// Maximum expected loss.
        /// </summary>
        public decimal? MaxLoss { get; set; }

        /// <summary>
        /// Bid value.
        /// </summary>
        public decimal? Bid { get; set; }

        /// <summary>
        /// Ask value.
        /// </summary>
        public decimal? Ask { get; set; }

        /// <summary>
        /// Bid/ask ratio.
        /// </summary>
        public decimal? BidAskRatio { get; set; }

        /// <summary>
        /// Latest option volume value.
        /// </summary>
        public decimal? Volume { get; set; }

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
        /// Implied volatility percentile.
        /// </summary>
        public decimal? ImpliedVolatilityPercentile { get; set; }

        /// <summary>
        /// Implied volatility rank.
        /// </summary>
        public decimal? ImpliedVolatilityRank { get; set; }

        /// <summary>
        /// Rank by selected strategies.
        /// Calculated based on the Return on Investment value.
        /// </summary>
        public int? StrategyRank { get; set; }

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
        /// Number of active contracts.
        /// </summary>
        public decimal? OpenInterest { get; set; }

        /// <summary>
        /// The parameter shows how the market copes with the sale to everyone (Volume/OpenInterest)
        /// </summary>
        public decimal? VolumeOpenInterest { get; set; }

        /// <summary>
        /// One day implied volatility rank change value.
        /// </summary>
        public decimal? OneDayImpliedVolatilityRankChange { get; set; }

        /// <summary>
        /// One week implied volatility rank change value.
        /// </summary>
        public decimal? OneWeekImpliedVolatilityRankChange { get; set; }

        /// <summary>
        /// Earnings date.
        /// </summary>
        public DateTime? EarningsDate { get; set; }

        /// <summary>
        /// Announced dividend dates.
        /// </summary>
        public List<DateTime> DividendDates { get; set; }
    }
}