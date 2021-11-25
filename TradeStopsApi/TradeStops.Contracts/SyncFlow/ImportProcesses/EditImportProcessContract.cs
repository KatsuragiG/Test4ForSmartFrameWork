using System;
using TradeStops.Common.Enums;
using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Initialize only fields you want to patch
    /// </summary>
    public class EditImportProcessContract
    {
        /// <summary>
        /// Import process status.
        /// </summary>
        public Optional<ImportProcessStatusTypes> ImportProcessStatusType { get; set; }

        /// <summary>
        /// Import process progress.
        /// </summary>
        public Optional<ImportProgressTypes> ImportProgressType { get; set; }

        /// <summary>
        /// Error description.
        /// </summary>
        public Optional<string> ErrorDescription { get; set; }

        /// <summary>
        /// Import date.
        /// </summary>
        public Optional<DateTime> ImportDate { get; set; }

        /// <summary>
        /// Defines that user was informed about the import.
        /// </summary>
        public Optional<bool> IsUserInformed { get; set; }

        /// <summary>
        /// Vendor sync error message ID.
        /// </summary>
        public Optional<int> VendorSyncErrorMessageId { get; set; }

        /// <summary>
        /// Defines that portfolio limit was exceeded.
        /// </summary>
        public Optional<bool> PortfoliosLimitExceeded { get; set; }
    }
}
