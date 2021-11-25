using System;
using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Initialize only fields you want to patch
    /// </summary>
    public class EditSyncPositionContract : EditPositionContract
    {
        /// <summary>
        ///  Vendor holding ID.
        /// </summary>
        public Optional<string> VendorHoldingId { get; set; }

        /// <summary>
        ///  Vendor symbol.
        /// </summary>
        public Optional<string> VendorSymbol { get; set; }

        /// <summary>
        ///  Date when position is marked as possibly closed.
        /// </summary>
        public Optional<DateTime?> PossibleCloseDate { get; set; }

        /// <summary>
        ///  Defines was user notified by email that position was marked as possibly closed.
        /// </summary>
        public Optional<bool?> PossiblyClosedNotificationSent { get; set; }

        /// <summary>
        ///  Defines that user can manually edit shares of sync position.
        /// </summary>
        public Optional<bool?> ChangingSharesPermission { get; set; }
    }
}
