using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Subtrade (part of Pair Trade position)
    /// </summary>
    public class SubtradeContract
    {
        /// <summary>
        /// Subtrade ID.
        /// </summary>
        public int SubtradeId { get; set; }

        /// <summary>
        /// Position ID.
        /// </summary>
        public int PositionId { get; set; }

        /// <summary>
        /// Position symbol ID.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Values for the symbol.
        /// </summary>
        public SymbolContract Symbol { get; set; }

        /// <summary>
        /// Position purchase date.
        /// </summary>
        public DateTime PurchaseDate { get; set; }

        /// <summary>
        /// Position purchase price not adjusted by corporate actions.
        /// </summary>
        public decimal PurchasePrice { get; set; }

        /// <summary>
        /// Purchase price adjusted by all corporate actions.
        /// </summary>
        public decimal PurchasePriceAdj { get; set; }

        /// <summary>
        /// Purchase price not adjusted by dividends.
        /// </summary>
        public decimal SplitsAdj { get; set; }

        /// <summary>
        /// Number of the subtrade shares.
        /// </summary>
        public decimal Shares { get; set; }

        /// <summary>
        /// Internal TradeStops value.
        /// </summary>
        public decimal InitialShares { get; set; }

        /// <summary>
        /// Subtrade trade type.
        /// </summary>
        public TradeTypes TradeType { get; set; }

        /// <summary>
        /// Subtrade status type.
        /// </summary>
        public PositionStatusTypes StatusType { get; set; }

        /// <summary>
        /// Position is deleted in the database.
        /// </summary>
        public bool Delisted { get; set; }

        /// <summary>
        /// The number of calendar days since the Entry Date.
        /// </summary>
        public int HoldPeriod { get; set; }

        /// <summary>
        /// Subtrade cost basis.
        /// </summary>
        public decimal CostBasis { get; set; }

        /// <summary>
        /// Adjusted total dividends value per share.
        /// </summary>
        public decimal DividendAdj { get; set; }

        /// <summary>
        /// Total dividends value.
        /// </summary>
        public decimal DividendsTotal { get; set; }

        /// <summary>
        /// Subtrade close date.
        /// </summary>
        public DateTime CloseDate { get; set; }

        /// <summary>
        /// Subtrade latest close price.
        /// </summary>
        public decimal ClosePrice { get; set; }

        /// <summary>
        /// ID of the position currency.
        /// </summary>
        public int CurrencyId { get; set; }
    }
}
