using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Statistics data for a vendor sync error.
    /// </summary>
    public class AdminSyncReportsErrorStatisticsContract
    {
        /// <summary>
        ///  Error code.
        /// </summary>
        public YodleeErrorCodeTypes? ErrorCode { get; set; }

        /// <summary>
        ///  Error name.
        /// </summary>
        public string ErrorName { get; set; }

        /// <summary>
        ///  Error description.
        /// </summary>
        public string ErrorDescription { get; set; }

        /// <summary>
        ///  Errors count.
        /// </summary>
        public int ErrorsCount { get; set; }

        /// <summary>
        ///  Errors percentage.
        /// </summary>
        public decimal ErrorsPercentage { get; set; }
    }
}
