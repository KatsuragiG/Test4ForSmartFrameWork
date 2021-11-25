using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create Pure Quant task.
    /// </summary>
    public class CreatePureQuantTaskContract
    {
        /// <summary>
        /// Pure Quant source parameters.
        /// </summary>
        public CreatePureQuantSourcesContract Sources { get; set; }

        /// <summary>
        /// Pure Quant filters parameters.
        /// </summary>
        public CreatePureQuantFiltersContract Filters { get; set; }

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
        public virtual bool AdjustByDividends { get; set; }

        /// <summary>
        /// Type of a strategy for the Pure Quant.
        /// </summary>
        public virtual QuantToolStrategyTypes StrategyType { get; set; }
    }
}
