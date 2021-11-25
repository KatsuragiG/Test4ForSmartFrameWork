using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create deleted sync position that will prevent synchronization for some position from synchronized portfolio
    /// </summary>
    public class CreateDeletedSyncPositionContract
    {
        /// <summary>
        ///  Vendor holding ID.
        /// </summary>
        public string VendorHoldingId { get; set; }

        /// <summary>
        ///  Vendor holding symbol.
        /// </summary>
        public string VendorSymbol { get; set; }

        /// <summary>
        ///  Position trade type (Long or Short).
        /// </summary>
        public TradeTypes TradeType { get; set; }

        /// <summary>
        ///  Portfolio ID.
        /// </summary>
        public int PortfolioId { get; set; }
    }
}
