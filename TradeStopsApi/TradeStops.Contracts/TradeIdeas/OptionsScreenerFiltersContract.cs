using System.Collections.Generic;

using TradeStops.Common.DataStructures;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to specify filters that will be applied to selected sources.
    /// </summary>
    public class OptionsScreenerFiltersContract
    {
        /// <summary>
        /// Search for options with specific price types.
        /// </summary>
        public List<OptionPriceTypes> OptionPriceTypes { get; set; }

        /// <summary>
        /// Search for options with specific decay types.
        /// </summary>
        public List<DecayTypes> DecayTypes { get; set; }

        /// <summary>
        /// List of strategies to apply as filter.
        /// </summary>
        public List<InvestmentStrategyTypes> StrategyIds { get; set; }

        /// <summary>
        /// Specifies how to apply multiple Strategies in the filter.
        /// </summary>
        public UnionTypes StrategiesUnionType { get; set; }

        /// <summary>
        /// Search for options that have qualified for selected strategies since provided date.
        /// </summary>
        public DateTimeFilter QualifiedForStrategiesFilter { get; set; }

        /// <summary>
        /// Search for options with expiration date withing specified date range.
        /// </summary>
        public DecimalFilter DaysToExpiration { get; set; }

        /// <summary>
        /// Filter by strike price value.
        /// </summary>
        public DecimalFilter StrikePrice { get; set; }

        /// <summary>
        /// Filter by moneyness value.
        /// </summary>
        public DecimalFilter Moneyness { get; set; }

        /// <summary>
        /// Filter by probability of profit.
        /// </summary>
        public DecimalFilter ProbabilityOfProfit { get; set; }

        /// <summary>
        /// Filter by position size.
        /// </summary>
        public DecimalFilter PositionSize { get; set; }

        /// <summary>
        /// Filter by option latest close price.
        /// </summary>
        public DecimalFilter LatestClosePrice { get; set; }

        /// <summary>
        /// Filter by latest close price of underlying stock.
        /// </summary>
        public DecimalFilter UnderlyingClosePrice { get; set; }

        /// <summary>
        /// Filter by return on investment.
        /// </summary>
        public DecimalFilter Roi { get; set; }

        /// <summary>
        /// Filter by maximum expected profit.
        /// </summary>
        public DecimalFilter MaxProfit { get; set; }

        /// <summary>
        /// Filter by maximum expected loss.
        /// </summary>
        public DecimalFilter MaxLoss { get; set; }

        /// <summary>
        /// Filter by bid value.
        /// </summary>
        public DecimalFilter Bid { get; set; }

        /// <summary>
        /// Filter by ask value.
        /// </summary>
        public DecimalFilter Ask { get; set; }

        /// <summary>
        /// Filter by bid/ask ratio.
        /// </summary>
        public DecimalFilter BidAskRatio { get; set; }

        /// <summary>
        /// Filter by latest option volume value.
        /// </summary>
        public DecimalFilter Volume { get; set; }

        /// <summary>
        /// Filter by implied volatility value.
        /// </summary>
        public DecimalFilter ImpliedVolatility { get; set; }

        /// <summary>
        /// Filter by historical volatility value.
        /// </summary>
        public DecimalFilter HistoricalVolatility { get; set; }

        /// <summary>
        /// Filter by implied/historical volatility ratio.
        /// </summary>
        public DecimalFilter VolatilityRatio { get; set; }

        /// <summary>
        /// Filter by implied volatility percentile.
        /// </summary>
        public DecimalFilter ImpliedVolatilityPercentile { get; set; }

        /// <summary>
        /// Filter by implied volatility rank.
        /// </summary>
        public DecimalFilter ImpliedVolatilityRank { get; set; }

        /// <summary>
        /// Filter by delta value.
        /// </summary>
        public DecimalFilter Delta { get; set; }

        /// <summary>
        /// Filter by gamma value.
        /// </summary>
        public DecimalFilter Gamma { get; set; }

        /// <summary>
        /// Filter by theta value.
        /// </summary>
        public DecimalFilter Theta { get; set; }

        /// <summary>
        /// Filter by vega value.
        /// </summary>
        public DecimalFilter Vega { get; set; }

        /// <summary>
        /// Filter by rho value.
        /// </summary>
        public DecimalFilter Rho { get; set; }

        /// <summary>
        /// Filter by open interest value.
        /// </summary>
        public DecimalFilter OpenInterest { get; set; }

        /// <summary>
        /// Filter by volume/open interest value.
        /// </summary>
        public DecimalFilter VolumeOpenInterest { get; set; }

        /// <summary>
        /// Filter by one day implied volatility rank change value.
        /// </summary>
        public DecimalFilter OneDayImpliedVolatilityRankChange { get; set; }

        /// <summary>
        /// Filter by one week implied volatility rank change value.
        /// </summary>
        public DecimalFilter OneWeekImpliedVolatilityRankChange { get; set; }

        /// <summary>
        /// List of ssi statuses of underlying stock.
        /// </summary>
        public List<SsiStatuses> SsiTypes { get; set; }

        /// <summary>
        /// List of trend types for underlying stock.
        /// </summary>
        public List<TrendTypes> TrendTypes { get; set; }

        /// <summary>
        /// Filter by Entry Date for underlying stock.
        /// </summary>
        public DateTimeFilter EntryDateFilter { get; set; }

        /// <summary>
        /// Filter by Early Entry Date for underlying stock.
        /// </summary>
        public DateTimeFilter EarlyEntryDateFilter { get; set; }

        /// <summary>
        /// Filter by latest RSI(14) value  for underlying stock.
        /// </summary>
        public DecimalFilter LatestRsi14Filter { get; set; }

        /// <summary>
        /// List of underlying stock strategies to apply as a filter.
        /// </summary>
        public List<InvestmentStrategyTypes> UnderlyingStockStrategyIds { get; set; }

        /// <summary>
        /// Specifies how to apply multiple underlying stock strategies in the filter.
        /// </summary>
        public UnionTypes UnderlyingStockStrategiesUnionType { get; set; }

        /// <summary>
        /// Search for options that have qualified for selected underlying stock strategies since provided date.
        /// </summary>
        public DateTimeFilter QualifiedForUnderlyingStockStrategiesFilter { get; set; }

        /// <summary>
        /// Filter by percentage change of underlying stock for the day.
        /// </summary>
        public DecimalFilter OneDayChangeInPercent { get; set; }

        /// <summary>
        /// Filter by percentage change of underlying stock for the week.
        /// </summary>
        public DecimalFilter OneWeekChangeInPercent { get; set; }

        /// <summary>
        /// Filter by percentage change of underlying stock for the month.
        /// </summary>
        public DecimalFilter OneMonthChangeInPercent { get; set; }

        /// <summary>
        /// Filter by dividends before expiration.
        /// </summary>
        public bool? DividendsBeforeExpiration { get; set; }

        /// <summary>
        /// Filter by earnings before expiration.
        /// </summary>
        public bool? EarningsBeforeExpiration { get; set; }

        /// <summary>
        /// Filter by global ranks of underlying stock.
        /// </summary>
        public List<GlobalRankTypes> GlobalRanks { get; set; }

        /// <summary>
        /// Filter by timing turn area of underlying stock.
        /// </summary>
        public List<TimingTurnAreaTypes> TimingTurnAreaTypes { get; set; }

        /// <summary>
        /// Filter by timing turn strength of underlying stock.
        /// </summary>
        public List<TimingTurnStrengthTypes> TimingTurnStrengthTypes { get; set; }

        /// <summary>
        /// Filter by timing series of underlying stock.
        /// </summary>
        public List<TimingSerieTypes> TimingSerieTypes { get; set; }

        /// <summary>
        /// Filter by option operations.
        /// </summary>
        public List<OptionOperationTypes> OptionOperationTypes { get; set; }
    }
}
