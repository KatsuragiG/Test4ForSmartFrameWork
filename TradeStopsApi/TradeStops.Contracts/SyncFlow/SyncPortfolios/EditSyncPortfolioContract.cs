using System;
using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Contract with synchronized portfolio fields to patch
    /// </summary>
    public class EditSyncPortfolioContract : EditPortfolioContract
    {
        /// <summary>
        /// (optional) Imported portfolio total value.
        /// </summary>
        public Optional<decimal> VendorPortfolioTotalValue { get; set; }

        /// <summary>
        /// (optional) Portfolio update date.
        /// </summary>
        public Optional<DateTime> UpdateDate { get; set; }

        /// <summary>
        /// (optional) Defines are credentials still up-to-date.
        /// </summary>
        public Optional<bool> IsRequireNewCredentials { get; set; }

        /// <summary>
        /// (optional) Date when last email related to errors on portfolio synchronization was sent.
        /// </summary>
        public Optional<DateTime?> LastEmailDate { get; set; }

        /// <summary>
        /// (optional) Number of emails related to errors on portfolio synchronization sent.
        /// </summary>
        public Optional<int?> NumberOfSentEmails { get; set; }

        /// <summary>
        /// (optional) Defines whether show info popup.
        /// </summary>
        public Optional<int?> ShowInfoPopupForError { get; set; }

        /// <summary>
        /// (optional) Defines status for update credentials process.
        /// </summary>
        public Optional<string> UpdateCredentialsStatus { get; set; }
    }
}
