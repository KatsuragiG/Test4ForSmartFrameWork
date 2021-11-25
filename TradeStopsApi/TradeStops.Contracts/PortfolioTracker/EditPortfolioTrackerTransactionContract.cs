using System;

using TradeStops.Common.Enums;
using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to edit portfolio tracker transaction.
    /// </summary>
    public class EditPortfolioTrackerTransactionContract
    {
        /// <summary>
        /// (Optional) Type of transaction.
        /// </summary>
        public Optional<PositionTransactionTypes> TransactionType { get; set; }

        /// <summary>
        /// (Optional) Number of shares, units, contracts, etc.
        /// </summary>
        public Optional<decimal> Quantity { get; set; }

        /// <summary>
        /// (Optional) Transaction quantity adjustment type.
        /// </summary>
        public Optional<SharesAdjustmentTypes> QuantityAdjustmentType { get; set; }

        /// <summary>
        /// (Optional) Transaction Price.
        /// </summary>
        public Optional<decimal> Price { get; set; }

        /// <summary>
        /// (Optional) Purchase price adjustment type.
        /// </summary>
        public Optional<PriceAdjustmentTypes> PriceAdjustmentType { get; set; }

        /// <summary>
        /// (Optional) The date when transaction happened.
        /// </summary>
        public Optional<DateTime> TradeDate { get; set; }
    }
}
