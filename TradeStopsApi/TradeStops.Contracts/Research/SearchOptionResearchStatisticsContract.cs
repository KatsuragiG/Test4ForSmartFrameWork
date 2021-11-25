using TradeStops.Common.DataStructures;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to get option research statistic.
    /// </summary>
    public class SearchOptionResearchStatisticsContract
    {
        /// <summary>
        /// Parent Symbol ID.
        /// </summary>
        public int ParentSymbolId { get; set; }

        /// <summary>
        /// Trade type.
        /// </summary>
        public TradeTypes TradeType { get; set; }

        /// <summary>
        /// Option Type.
        /// </summary>
        public OptionTypes OptionType { get; set; }

        /// <summary>
        /// Search for options with expiration date withing specified date range.
        /// </summary>
        public DecimalFilter DaysToExpiration { get; set; }

        /// <summary>
        /// Search by strike price value.
        /// </summary>
        public DecimalFilter StrikePrice { get; set; }
    }
}
