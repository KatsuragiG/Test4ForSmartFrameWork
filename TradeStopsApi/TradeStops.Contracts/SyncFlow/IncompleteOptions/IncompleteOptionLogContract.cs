using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Option with incomplete data
    /// </summary>
    public class IncompleteOptionLogContract
    {
        /// <summary>
        ///  Incomplete option log ID.
        /// </summary>
        public int IncompleteOptionLogId { get; set; }

        /// <summary>
        ///  Vendor account ID.
        /// </summary>
        public string VendorAccountId { get; set; }

        /// <summary>
        ///  Vendor portfolio ID.
        /// </summary>
        public string VendorPortfolioId { get; set; }

        /// <summary>
        ///  Vendor holding ID.
        /// </summary>
        public string VendorHoldingId { get; set; }

        /// <summary>
        ///  Holding symbol.
        /// </summary>
        public string VendorSymbol { get; set; }

        /// <summary>
        ///  Holding description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///  Option expiration date.
        /// </summary>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        ///  Option strike price.
        /// </summary>
        public decimal? StrikePrice { get; set; }

        /// <summary>
        ///  Option type.
        /// </summary>
        public OptionTypes? OptionType { get; set; }

        /// <summary>
        ///  Incomplete option status.
        /// </summary>
        public IncompleteOptionStatusTypes IncompleteOptionStatusType { get; set; }

        /// <summary>
        ///  Financial institution name.
        /// </summary>
        public string FinancialInstitutionName { get; set; }

        /// <summary>
        ///  Incomplete option creation date in UTC.
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        ///  TradeSmith user ID.
        /// </summary>
        public int TradeSmithUserId { get; set; }

        /// <summary>
        ///  User email address.
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        ///  Vendor type.
        /// </summary>
        public VendorTypes VendorTypeId { get; set; }
    }
}
