using System;
using TradeStops.Common.Enums;
using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to edit unconfirmed position
    /// </summary>
    public class EditUnconfirmedPositionContract
    {
        /// <summary>
        /// (required) Unconfirmed position ID.
        /// </summary>
        public int UnconfirmedPositionId { get; set; }

        /// <summary>
        /// Vendor symbol.
        /// </summary>
        public Optional<string> VendorSymbol { get; set; }

        /// <summary>
        ///  Parsed holding symbol.
        /// </summary>
        public Optional<string> ParsedHoldingSymbol { get; set; }

        /// <summary>
        /// Unconfirmed position trade type.
        /// </summary>
        public Optional<TradeTypes> TradeType { get; set; }

        /// <summary>
        /// Unconfirmed symbol type in the TradeStops.
        /// </summary>
        public Optional<string> DataType { get; set; }

        /// <summary>
        /// Position purchase date.
        /// </summary>
        public Optional<DateTime> PurchaseDate { get; set; }

        /// <summary>
        /// Position purchase price.
        /// </summary>
        public Optional<decimal> PurchasePrice { get; set; }

        /// <summary>
        /// Position purchase fee
        /// </summary>
        public Optional<decimal> OpenFee { get; set; }

        /// <summary>
        /// Options expiration date. Available only for options.
        /// </summary>
        public Optional<DateTime> ExpirationDate { get; set; }

        /// <summary>
        /// Option strike price. Available only for options.
        /// </summary>
        public Optional<decimal> StrikePrice { get; set; }

        /// <summary>
        /// Option type. Available only for options.
        /// </summary>
        public Optional<OptionTypes?> OptionType { get; set; }

        /// <summary>
        /// Unconfirmed position shares number.
        /// </summary>
        public Optional<decimal> Shares { get; set; }

        /// <summary>
        ///  Vendor holding ID.
        /// </summary>
        public Optional<string> VendorHoldingId { get; set; }

        /// <summary>
        /// Internal TradeStops value. Position has been lost from data provided response.
        /// </summary>
        public Optional<DateTime?> PossibleCloseDate { get; set; }

        /// <summary>
        /// Internal TradeStops value. User has been notified about possible closing event by email.
        /// </summary>
        public Optional<bool?> PossiblyClosedNotificationSent { get; set; }

        /// <summary>
        ///  Defines that user can manually edit shares of unconfirmed position.
        /// </summary>
        public Optional<bool> ChangingSharesPermission { get; set; }

        /// <summary>
        /// (optional) New currency identifier for the unconfirmed position.
        /// </summary>
        public Optional<string> CurrencyName { get; set; }
    }
}
