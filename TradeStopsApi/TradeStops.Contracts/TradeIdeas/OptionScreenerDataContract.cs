using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Option screener data.
    /// </summary>
    public class OptionScreenerDataContract
    {
        /// <summary>
        /// Symbol ID.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Parent symbol ID.
        /// </summary>
        public int ParentSymbolId { get; set; }

        /// <summary>
        /// Option strategy.
        /// </summary>
        public InvestmentStrategyTypes? StrategyType { get; set; }

        /// <summary>
        /// Trade Type.
        /// </summary>
        public TradeTypes TradeType { get; set; }

        /// <summary>
        /// Maximum expected profit.
        /// </summary>
        public decimal? MaxProfit { get; set; }

        /// <summary>
        /// Maximum expected loss.
        /// </summary>
        public decimal? MaxLoss { get; set; }

        /// <summary>
        /// Position size value.
        /// </summary>
        public decimal? PositionSize { get; set; }

        /// <summary>
        /// Probability of profit value.
        /// </summary>
        public decimal? ProbabilityOfProfit { get; set; }

        /// <summary>
        /// ROI value.
        /// </summary>
        public decimal? Roi { get; set; }
    }
}
