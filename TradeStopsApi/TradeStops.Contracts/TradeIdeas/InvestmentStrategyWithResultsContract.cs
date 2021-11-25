using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Contract with basic information about Investment Strategy and with the list of its results
    /// </summary>
    public class InvestmentStrategyWithResultsContract
    {
        /// <summary>
        /// The enumeration value for the strategy, the same as the value from input
        /// </summary>
        public InvestmentStrategyTypes StrategyId { get; set; }

        /// <summary>
        /// The name of the strategy
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The list of results
        /// </summary>
        public List<InvestmentStrategyResultContract> Results { get; set; }
    }
}
