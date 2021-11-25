using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to get options research statistic.
    /// </summary>
    public class GetBulkOptionResearchStatisticContract
    {
        /// <summary>
        /// Symbol IDs.
        /// </summary>
        public List<int> SymbolIds { get; set; }

        /// <summary>
        /// Trade type.
        /// </summary>
        public TradeTypes TradeType { get; set; }
    }
}
