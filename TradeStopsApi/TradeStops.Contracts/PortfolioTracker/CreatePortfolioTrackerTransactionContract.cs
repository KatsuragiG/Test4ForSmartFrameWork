using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create portfolio tracker transaction
    /// </summary>
    public class CreatePortfolioTrackerTransactionContract
    {
        /// <summary>
        ///  Type of transaction
        /// </summary>
        public PositionTransactionTypes TransactionType { get; set; }        

        /// <summary>
        /// Number of shares, units, contracts, etc.
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Transaction quantity adjustment type.
        /// </summary>
        public SharesAdjustmentTypes QuantityAdjustmentType { get; set; }

        /// <summary>
        /// Transaction Price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Purchase price adjustment type.
        /// </summary>
        public PriceAdjustmentTypes PriceAdjustmentType { get; set; }

        /// <summary>
        /// The date when transaction happened.
        /// </summary>
        public DateTime TradeDate { get; set; }
    }
}
