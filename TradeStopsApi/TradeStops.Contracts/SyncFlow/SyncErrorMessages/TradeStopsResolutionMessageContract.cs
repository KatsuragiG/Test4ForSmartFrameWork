using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Yodlee TradeStops resolution message
    /// </summary>
    public class TradeStopsResolutionMessageContract
    {
        /// <summary>
        ///  Tradestops error resolution type for yodlee error.
        /// </summary>
        public SyncResolutionTypes ResolutionType { get; set; }

        /// <summary>
        ///  Tradestops error resolution title for yodlee error.
        /// </summary>
        public string ResolutionTitle { get; set; }

        /// <summary>
        ///  Tradestops error resolution message for yodlee error.
        /// </summary>
        public string ResolutionMessage { get; set; }
    }
}
