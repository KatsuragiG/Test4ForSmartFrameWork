using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create option with incomplete data. Used in portfolio synchronization.
    /// </summary>
    public class CreateIncompleteOptionLogContract
    {
        /// <summary>
        /// Vendor account ID.
        /// </summary>
        public string VendorAccountId { get; set; }

        /// <summary>
        /// Vendor portfolio ID.
        /// </summary>
        public string VendorPortfolioId { get; set; }

        /// <summary>
        /// Vendor holding ID.
        /// </summary>
        public string VendorHoldingId { get; set; }

        /// <summary>
        /// Holding symbol.
        /// </summary>
        public string VendorSymbol { get; set; }

        /// <summary>
        /// (optional) Holding description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// (optional) Option expiration date.
        /// </summary>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// (optional) Option strike price.
        /// </summary>
        public decimal? StrikePrice { get; set; }

        /// <summary>
        /// Option type.
        /// </summary>
        public OptionTypes? OptionType { get; set; }

        /// <summary>
        /// Incomplete option status.
        /// </summary>
        public IncompleteOptionStatusTypes IncompleteOptionStatusType { get; set; }

        /// <summary>
        /// Financial institution ID.
        /// </summary>
        public int FinancialInstitutionId { get; set; }
    }
}
