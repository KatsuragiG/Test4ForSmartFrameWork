using System;
using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to save Pure Quant position result
    /// </summary>
    public class CreatePureQuantResultPositionContract
    {
        /// <summary>
        /// ID of the symbol.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// ID of the position currency.
        /// </summary>
        public int CurrencyId { get; set; }

        /// <summary>
        /// Latest price of the symbol.
        /// </summary>
        public decimal LatestPrice { get; set; }

        /// <summary>
        /// Percentage amount of risk in the individual position compared to the value of the overall portfolio.
        /// </summary>
        public decimal PositionRisk { get; set; }

        /// <summary>
        /// Amount of money invested in each position of the portfolio.
        /// </summary>
        public decimal PositionSize { get; set; }

        /// <summary>
        /// Percentage of each position in the portfolio.
        /// </summary>
        public decimal PositionSizePercent { get; set; }

        /// <summary>
        /// Recommended size share to take an equal risk per position.
        /// </summary>
        public decimal SuggestedShares { get; set; }

        /// <summary>
        /// Position has been included into the Risk Rebalancer Algorithm.
        /// </summary>
        public bool RebalancedPosition { get; set; }

        /// <summary>
        /// Rank of the position in Quant Tool.
        /// </summary>
        public int Rank { get; set; }

        /// <summary>
        /// Position Pure Quant sources.
        /// </summary>
        public List<string> Sources { get; set; }

        /// <summary>
        /// Company sector name.
        /// </summary>
        public string SectorName { get; set; }

        /// <summary>
        /// Position Pure Quant basket sources.
        /// </summary>
        [Obsolete("Do not use the property to save data. Left for compatibility with old PQ results.")]
        public List<BasketTypes> BasketSources { get; set; }
    }
}
