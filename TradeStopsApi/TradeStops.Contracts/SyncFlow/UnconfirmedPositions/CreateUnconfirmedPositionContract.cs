using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create unconfirmed position
    /// </summary>
    public class CreateUnconfirmedPositionContract
    {
        /// <summary>
        ///  Vendor holding ID.
        /// </summary>
        public string VendorHoldingId { get; set; }

        /// <summary>
        ///  Holding symbol name.
        /// </summary>
        public string ParsedHoldingSymbol { get; set; }

        /// <summary>
        ///  Portfolio ID.
        /// </summary>
        public int PortfolioId { get; set; }

        /// <summary>
        ///  Position trade type (Long or Short).
        /// </summary>
        public TradeTypes TradeType { get; set; }

        /// <summary>
        ///  Position symbol type.
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        ///  (optional) Position purchase date.
        /// </summary>
        public DateTime? PurchaseDate { get; set; }

        /// <summary>
        ///  (optional) Position purchase price.
        /// </summary>
        public decimal? PurchasePrice { get; set; }

        /// <summary>
        ///  (optional) Position close price.
        /// </summary>
        public decimal? ClosePrice { get; set; }

        /// <summary>
        ///  (optional) Cost basis.
        /// </summary>
        public decimal? CostBasis { get; set; }

        /// <summary>
        ///  Shares count.
        /// </summary>
        public decimal Shares { get; set; }

        /// <summary>
        ///  Vendor symbol.
        /// </summary>
        public string VendorSymbol { get; set; }

        /// <summary>
        ///  (optional) Option type.
        /// </summary>
        public OptionTypes? OptionType { get; set; }

        /// <summary>
        ///  (optional) Option strike price.
        /// </summary>
        public decimal? StrikePrice { get; set; }

        /// <summary>
        ///  (optional) Option expiration date.
        /// </summary>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        ///  Position has been combined with another one.
        /// </summary>
        public bool IsCombined { get; set; }

        /// <summary>
        /// Name of the unconfirmed position currency.
        /// </summary>
        public string CurrencyName { get; set; }

        /// <summary>
        /// Position open fee
        /// </summary>
        public decimal OpenFee { get; set; }
    }
}
