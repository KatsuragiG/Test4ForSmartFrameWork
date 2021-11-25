using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to initiate sync vendor account task
    /// </summary>
    public class InitiateSyncVendorAccountTaskContract
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
        /// Vendor account last refresh date.
        /// </summary>
        public DateTime LastRefreshDate { get; set; }

        /// <summary>
        /// Vendor account last error message on refresh.
        /// </summary>
        public int VendorSyncErrorMessageId { get; set; }

        /// <summary>
        /// Refresh type.
        /// </summary>
        public SynchronizationActionTypes SynchronizationActionType { get; set; }
    }
}
