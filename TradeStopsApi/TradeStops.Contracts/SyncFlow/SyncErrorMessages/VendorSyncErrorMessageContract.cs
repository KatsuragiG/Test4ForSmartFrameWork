namespace TradeStops.Contracts
{
    /// <summary>
    /// Sync error message
    /// </summary>
    public class VendorSyncErrorMessageContract
    {
        /// <summary>
        ///  Sync error message contract.
        /// </summary>
        public SyncErrorMessageContract ServerErrorMessage { get; set; }

        /// <summary>
        ///  Tradestops error message contract.
        /// </summary>
        public TradeStopsErrorMessageContract TradeStopsErrorMessage { get; set; }

        /// <summary>
        ///  Tradestops resolution message contract.
        /// </summary>
        public TradeStopsResolutionMessageContract TradeStopsResolutionMessage { get; set; }
    }
}
