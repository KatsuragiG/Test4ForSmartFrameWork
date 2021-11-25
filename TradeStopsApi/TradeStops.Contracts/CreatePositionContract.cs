using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    ////public interface IContractWithPriceAndShares
    ////{
    ////    decimal? PurchasePrice { get; set; }
    ////    PriceAdjustmentTypes PriceAdjustmentType { get; set; }
    ////    decimal InitialShares { get; set; }
    ////    SharesAdjustmentTypes SharesAdjustmentType { get; set; }
    ////}

    /// <summary>
    /// Parameters to create position
    /// </summary>
    public class CreatePositionContract // : IContractWithPriceAndShares
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
        /// Position purchase date.
        /// </summary>
        public DateTime? PurchaseDate { get; set; }

        /// <summary>
        /// Position purchase price.
        /// </summary>
        public decimal? PurchasePrice { get; set; }

        /// <summary>
        /// Purchase price adjustment type.
        /// </summary>
        public PriceAdjustmentTypes PriceAdjustmentType { get; set; } // todo for Anton: rename to PurchasePriceAdjustmentType, the same for editpositioncontract

        /// <summary>
        /// Position shares.
        /// </summary>
        public decimal InitialShares { get; set; } // todo for Anton: rename to shares

        /// <summary>
        /// Position shares adjustment type.
        /// </summary>
        public SharesAdjustmentTypes SharesAdjustmentType { get; set; }

        /// <summary>
        /// Position opening fee.
        /// </summary>
        public decimal OpenFee { get; set; }

        /// <summary>
        /// Ignore dividends in the position price adjustment and displaying.
        /// </summary>
        public bool IgnoreDividend { get; set; }

        /// <summary>
        /// Position trade type.
        /// </summary>
        public TradeTypes TradeType { get; set; }

        /// <summary>
        ///  Position status type.
        /// </summary>
        public PositionStatusTypes StatusType { get; set; }

        /// <summary>
        /// (optional) Position notes.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// (optional) Position close date (applicable only for closed positions).
        /// </summary>
        public DateTime? CloseDate { get; set; } // not required for open position

        /// <summary>
        /// (optional) Position close price (applicable only for closed positions).
        /// </summary>
        public decimal? ClosePrice { get; set; } // not required for open position

        /// <summary>
        /// (optional) Position close fee (applicable only for closed positions).
        /// </summary>
        public decimal? CloseFee { get; set; } // not required for open position

        /// <summary>
        /// Indicates whether intraday prices or trade close prices must be used. Must be false for CryptoStops, true for TradeStops
        /// </summary>
        public bool UseIntraday { get; set; }

        /// <summary>
        /// (optional) List of tags.
        /// </summary>
        public List<string> Tags { get; set; }
    }
}
