using System;
using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Pure Quant position result.
    /// </summary>
    public class PureQuantResultPositionContract
    {
        /// <summary>
        /// Position symbol.
        /// </summary>
        public SymbolContract Symbol { get; set; }

        /// <summary>
        /// Position currency.
        /// </summary>
        public CurrencyContract Currency { get; set; }

        /// <summary>
        /// Current symbol SSI.
        /// </summary>
        public SsiCurrentValueContract SsiCurrentValue { get; set; }

        /// <summary>
        /// Current symbol Volatility Quotient.
        /// </summary>
        public decimal VolatilityQuotient { get; set; }

        /// <summary>
        /// Current symbol Average 30-years Volatility Quotient.
        /// </summary>
        public decimal Average30YearsVolatilityQuotient { get; set; }

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
        [Obsolete("Left for compatibility with old PQ results.")]
        public List<BasketTypes> BasketSources { get; set; }
    }
}
