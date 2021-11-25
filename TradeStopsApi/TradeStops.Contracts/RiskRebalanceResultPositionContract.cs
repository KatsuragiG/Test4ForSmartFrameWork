using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class RiskRebalanceResultPositionContract
    {
        /// <summary>
        /// Position ID.
        /// </summary>
        public int PositionId { get; set; }

        /// <summary>
        /// Symbol ID.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Symbol.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Symbol name.
        /// </summary>
        public string SymbolName { get; set; }

        /// <summary>
        /// Total value of the position.
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// Percent value of the position in the portfolio.
        /// </summary>
        public decimal ValuePercent { get; set; }

        /// <summary>
        /// Share number of the symbol.
        /// </summary>
        public decimal Shares { get; set; }

        /// <summary>
        /// Latest price.
        /// </summary>
        public decimal LatestPrice { get; set; }

        /// <summary>
        /// Volatility Quotient (VQ) for the symbol.
        /// </summary>
        public decimal Vq { get; set; }

        /// <summary>
        /// Average VQ value fot 30 years
        /// </summary>
        public decimal Average30YearsVolatilityQuotient { get; set; }

        /// <summary>
        /// Share number of the symbol after rebalancing calculations.
        /// </summary>
        public decimal UpdatedShares { get; set; }

        /// <summary>
        /// Difference between current shares in the portfolio and adjusted shares number.
        /// </summary>
        public decimal SharesDifference { get; set; }

        /// <summary>
        /// Value of the asset weight by position of the portfolio value according to rebalancing calculations.
        /// </summary>
        public decimal UpdatedWeight { get; set; }

        /// <summary>
        /// Percent value of the asset weight by position of the portfolio value according to rebalancing calculations.
        /// </summary>
        public decimal UpdatedWeightPercent { get; set; }

        /// <summary>
        /// Currency Id.
        /// </summary>
        public int CurrencyId { get; set; }

        /// <summary>
        /// Currency Symbol.
        /// </summary>
        public string CurrencySymbol { get; set; }

        /// <summary>
        /// Sum of the position cost bases.
        /// </summary>
        public decimal? CostBasis { get; set; }

        /// <summary>
        /// Sum of the position cost bases.
        /// </summary>
        public DateTime? PurchaseDate { get; set; }

        /// <summary>
        /// Position purchase price not adjusted by corporate actions.
        /// </summary>
        public decimal? PurchasePrice { get; set; }

        /// <summary>
        /// Percent value of the current risk per position.
        /// </summary>
        public decimal CurrentRisk { get; set; }

        /// <summary>
        /// Percent value of the risk per position according to rebalancing calculations .
        /// </summary>
        public decimal UpdatedRisk { get; set; }

        /// <summary>
        /// Position Trade type.
        /// </summary>
        public TradeTypes TradeType { get; set; }

        /// <summary>
        /// Brokerage commission to open a position.
        /// </summary>
        public decimal OpenFee { get; set; }

        /// <summary>
        /// Dividends per share.
        /// </summary>
        public decimal DividendsPerShare { get; set; }

        /// <summary>
        /// Multiplicator value of the currency exchange.
        /// </summary>
        public decimal CrossCource { get; set; }

        /// <summary>
        /// Defines if position SSI state is   StoppedOut .
        /// </summary>
        public bool IsSsiStoppedOut { get; set; }

        /// <summary>
        /// Ssi values.
        /// </summary>
        public SsiValueContract Ssi { get; set; }

        /// <summary>
        /// Amount value to be invested according to rebalancing calculations.
        /// </summary>
        public decimal AmtInvested { get; set; }

        /// <summary>
        /// Marks that the stock is added.
        /// </summary>
        public bool IsAdded { get; set; }

        /// <summary>
        /// Determines if the added stok is locked and  included to rebalancing calculations without the adjustment in the result.
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// Determines whether to include dividends or not.
        /// </summary>
        public bool IgnoreDividend { get; set; }
    }
}
