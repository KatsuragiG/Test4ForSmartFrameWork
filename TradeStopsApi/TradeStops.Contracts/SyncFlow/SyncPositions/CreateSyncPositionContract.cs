using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create synchronized position
    /// </summary>
    public class CreateSyncPositionContract
    {
        /// <summary>
        ///  Portfolio ID.
        /// </summary>
        public int PortfolioId { get; set; }

        /// <summary>
        ///  Symbol ID.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        ///  (optional) Purchase date.
        /// </summary>
        public DateTime? PurchaseDate { get; set; }

        /// <summary>
        ///  (optional) Purchase price.
        /// </summary>
        public decimal? PurchasePrice { get; set; }

        /// <summary>
        ///  Shares count.
        /// </summary>
        public decimal Shares { get; set; }

        /// <summary>
        ///  Position open fee.
        /// </summary>
        public decimal OpenFee { get; set; }

        /// <summary>
        ///  Position trade type(Long or Short).
        /// </summary>
        public TradeTypes TradeType { get; set; }

        /// <summary>
        ///  Holding ID.
        /// </summary>
        public string HoldingId { get; set; }

        /// <summary>
        ///  Holding symbol.
        /// </summary>
        public string HoldingSymbol { get; set; }
    }
}
