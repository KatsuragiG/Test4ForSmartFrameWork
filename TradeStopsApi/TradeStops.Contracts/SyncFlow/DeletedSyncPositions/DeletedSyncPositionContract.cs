using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Deleted sync position that will prevent synchronization for some position from synchronized portfolio.
    /// </summary>
    public class DeletedSyncPositionContract
    {
        /// <summary>
        /// Deleted sync position ID.
        /// </summary>
        public int DeletedSyncPositionId { get; set; }

        /// <summary>
        ///  Vendor holding ID.
        /// </summary>
        public string VendorHoldingId { get; set; }

        /// <summary>
        ///  Vendor holding symbol.
        /// </summary>
        public string VendorSymbol { get; set; }

        /// <summary>
        ///  Position trade type(Long or Short).
        /// </summary>
        public TradeTypes TradeType { get; set; }
    }
}
