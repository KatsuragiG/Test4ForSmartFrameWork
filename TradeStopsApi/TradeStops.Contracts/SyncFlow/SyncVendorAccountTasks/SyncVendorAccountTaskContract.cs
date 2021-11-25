using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Sync vendor account task
    /// </summary>
    public class SyncVendorAccountTaskContract
    {
        /// <summary>
        ///  Sync vendor account task ID.
        /// </summary>
        public int SyncVendorAccountTaskId { get; set; }

        /// <summary>
        /// Sync vendor account task status.
        /// </summary>
        public SyncVendorAccountTaskStatusTypes SyncVendorAccountTaskStatus { get; set; }

        /// <summary>
        /// Error message ID which was used to initiate refresh task.
        /// </summary>
        public int VendorSyncErrorMessageId { get; set; }
    }
}
