using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Portfolio tracker transaction contract.
    /// </summary>
    public class PortfolioTrackerTransactionContract
    {
        /// <summary>
        /// Unique transaction Id.
        /// </summary>
        public int TransactionId { get; set; }

        /// <summary>
        /// Id of subtrade this transaction belongs to.
        /// </summary>
        public int SubtradeId { get; set; }

        /// <summary>
        /// Symbol contract
        /// </summary>
        public SymbolContract Symbol { get; set; }

        /// <summary>
        ///  Type of transaction
        /// </summary>
        public PositionTransactionTypes TransactionType { get; set; }

        /// <summary>
        /// Quantity value which is not adjusted by any corporate action.
        /// </summary>
        public decimal InitialQuantity { get; set; }

        /// <summary>
        /// Quantity value adjusted by all corporate action since TradeDate.
        /// </summary>
        public decimal QuantityAdj { get; set; }

        /// <summary>
        /// Transaction price value which is not adjusted by any corporate action.
        /// </summary>
        public decimal InitialPrice { get; set; }

        /// <summary>
        /// Transaction price value adjusted by all corporate actions since TradeDate.
        /// </summary>
        public decimal PriceAdj { get; set; }

        /// <summary>
        /// Transaction price value adjusted by all corporate actions except dividends since TradeDate.
        /// </summary>
        public decimal PriceSplitAdj { get; set; }

        /// <summary>
        /// The date when transaction happened.
        /// </summary>
        public DateTime TradeDate { get; set; }

        /// <summary>
        /// Indicates wheither transaction was manually deleted by user.
        /// </summary>
        public bool Delisted { get; set; }
    }
}
