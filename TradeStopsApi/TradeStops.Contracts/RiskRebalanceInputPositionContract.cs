using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class RiskRebalanceInputPositionContract
    {
        /// <summary>
        /// Position Id.
        /// </summary>
        public int PositionId { get; set; }

        /// <summary>
        /// Symbol Id.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// (optional) Purchase price not adjusted by dividends.
        /// </summary>
        public decimal? SplitAdjPurchasePrice { get; set; }

        /// <summary>
        /// (optional) Position purchase date.
        /// </summary>
        public DateTime? PurchaseDate { get; set; }

        /// <summary>
        /// Number of shares.
        /// </summary>
        public decimal Shares { get; set; }

        /// <summary>
        /// Position Trade type.
        /// </summary>
        public TradeTypes TradeType { get; set; }

        /// <summary>
        ///  Brokerage commission to open a position.
        /// </summary>
        public decimal OpenFee { get; set; }

        /// <summary>
        /// Dividends per share.
        /// </summary>
        public decimal DividendsPerShare { get; set; }

        /// <summary>
        /// Determines whether to include dividends or not.
        /// </summary>
        public bool IgnoreDividend { get; set; }

        /// <summary>
        /// Marks that the stock is added.
        /// </summary>
        public bool IsAdded { get; set; }

        /// <summary>
        /// Determines if the added stok is locked and  included to rebalancing calculations without the adjustment of the number of shares in the result.
        /// </summary>
        public bool IsLocked { get; set; }
    }
}
