using System;
using System.Collections.Generic;
using TradeStops.Common.Enums;
using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Initialize only fields you want to patch
    /// </summary>
    public class EditPositionContract
    {
        /// <summary>
        /// (required) Position ID.
        /// </summary>
        public int PositionId { get; set; }

        /// <summary>
        /// (optional) Symbol ID.
        /// </summary>
        public Optional<int> SymbolId { get; set; }

        /// <summary>
        /// (optional) Position purchase date.
        /// </summary>
        public Optional<DateTime?> PurchaseDate { get; set; }

        /// <summary>
        /// (optional) Position purchase price.
        /// </summary>
        public Optional<decimal?> PurchasePrice { get; set; }

        /// <summary>
        /// (optional) Position shares.
        /// </summary>
        public Optional<decimal> InitialShares { get; set; }

        /// <summary>
        /// (optional) Position opening fee.
        /// </summary>
        public Optional<decimal> OpenFee { get; set; }

        /// <summary>
        /// (optional) Ignore dividends in the position price adjustment and displaying.
        /// </summary>
        public Optional<bool> IgnoreDividend { get; set; }

        /// <summary>
        /// (optional) Position trade type.
        /// </summary>
        public Optional<TradeTypes> TradeType { get; set; }

        /// <summary>
        /// (optional) Position notes.
        /// </summary>
        public Optional<string> Notes { get; set; }

        /// <summary>
        /// (optional) Position close date (applicable only for closed positions).
        /// </summary>
        public Optional<DateTime> CloseDate { get; set; }

        /// <summary>
        /// (optional) Position close price (applicable only for closed positions).
        /// </summary>
        public Optional<decimal> ClosePrice { get; set; }

        /// <summary>
        /// (optional) Position close fee (applicable only for closed positions).
        /// </summary>
        public Optional<decimal> CloseFee { get; set; }

        /// <summary>
        /// (optional) Purchase price adjustment type.
        /// </summary>
        // todo: rename to PurchasePriceAdjustmentType
        public Optional<PriceAdjustmentTypes> PriceAdjustmentType { get; set; }

        /// <summary>
        /// (optional) Shares adjustment type.
        /// </summary>
        public Optional<SharesAdjustmentTypes> SharesAdjustmentType { get; set; }

        /// <summary>
        /// (optional) Tags.
        /// </summary>
        public Optional<List<string>> Tags { get; set; }
    }
}
