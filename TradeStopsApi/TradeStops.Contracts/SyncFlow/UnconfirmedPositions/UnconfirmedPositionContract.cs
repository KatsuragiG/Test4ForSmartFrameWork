using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Unconfirmed position
    /// </summary>
    public class UnconfirmedPositionContract
    {
        /// <summary>
        /// Unconfirmed position Id.
        /// </summary>
        public int UnconfirmedPositionId { get; set; }

        /// <summary>
        /// Internal TradeStops value. ID of the position in Vendor system.
        /// </summary>
        public string VendorHoldingId { get; set; }

        /// <summary>
        /// Vendor symbol name.
        /// </summary>
        public string VendorSymbol { get; set; }

        /// <summary>
        /// Close price value of the unconfirmed position.
        /// </summary>
        public decimal? ClosePrice { get; set; }

        /// <summary>
        /// Position purchase date.
        /// </summary>
        public DateTime? PurchaseDate { get; set; }

        /// <summary>
        /// Shares number.
        /// </summary>
        public decimal Shares { get; set; }

        /// <summary>
        /// Position trade type.
        /// </summary>
        public TradeTypes TradeType { get; set; }

        /// <summary>
        /// Position purchase price.
        /// </summary>
        public decimal? PurchasePrice { get; set; }

        /// <summary>
        /// Brokerage commission to open a position.
        /// </summary>
        public decimal? OpenFee { get; set; }

        /// <summary>
        /// Portfolio Id.
        /// </summary>
        public int PortfolioId { get; set; }

        /// <summary>
        /// Unconfirmed position value.
        /// </summary>
        public decimal? Value { get; set; }

        /// <summary>
        /// Options expiration date. Available only for options.
        /// </summary>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Option strike price. Available only for options.
        /// </summary>
        public decimal? StrikePrice { get; set; }

        /// <summary>
        /// Option type. Available only for options.
        /// </summary>
        public OptionTypes? StrikeType { get; set; }

        /// <summary>
        /// Position symbol type (StockData or OptionsData).
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// Unconfirmed position cost basis.
        /// </summary>
        public decimal? CostBasis { get; set; }

        /// <summary>
        /// Internal TradeStops value. Parsed holding symbol.
        /// </summary>
        public string ParsedHoldingSymbol { get; set; }

        /// <summary>
        /// Internal TradeStops value. Position has been combined with another one.
        /// </summary>
        public bool? IsCombined { get; set; }

        /// <summary>
        /// Internal TradeStops value. Position has been lost from data provided response.
        /// </summary>
        public DateTime? PossibleCloseDate { get; set; }

        /// <summary>
        /// Internal TradeStops value. User has been notified about possible closing event by email.
        /// </summary>
        public bool? PossiblyClosedNotificationSent { get; set; }

        /// <summary>
        /// Internal TradeStops value. Determines if it's possible to change shares of the position.
        /// </summary>
        public bool ChangingSharesPermission { get; set; }

        /// <summary>
        /// Date when position was created in the database
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Currency name.
        /// </summary>
        public string CurrencyName { get; set; }
    }
}
