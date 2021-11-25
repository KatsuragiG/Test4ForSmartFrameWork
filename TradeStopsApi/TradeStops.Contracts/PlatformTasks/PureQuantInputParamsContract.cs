using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Contract with saved Pure Quant input parameters.
    /// </summary>
    public class PureQuantInputParamsContract
    {
        /// <summary>
        /// Pure Quant source parameters.
        /// </summary>
        public PureQuantSourcesInputContract Sources { get; set; }

        /// <summary>
        /// Pure Quant filters parameters.
        /// </summary>
        public PureQuantFiltersInputContract Filters { get; set; }

        /// <summary>
        /// Investment amount.
        /// </summary>
        public decimal InvestmentAmount { get; set; }

        /// <summary>
        /// Investment currency.
        /// </summary>
        public int InvestmentCurrencyId { get; set; }

        /// <summary>
        /// Number of positions in the result.
        /// </summary>
        public int? PositionsCount { get; set; }

        /// <summary>
        /// Determines if the prices adjusted by dividends will be used in calculations.
        /// </summary>
        public bool AdjustByDividends { get; set; }

        /// <summary>
        /// Type of a strategy for the Pure Quant.
        /// </summary>
        public QuantToolStrategyTypes StrategyType { get; set; }
    }
}
