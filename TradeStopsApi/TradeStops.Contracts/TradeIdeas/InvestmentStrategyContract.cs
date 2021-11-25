using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Investment strategy information
    /// </summary>
    public class InvestmentStrategyContract
    {
        /// <summary>
        /// The enumeration value for the strategy ID
        /// </summary>
        public InvestmentStrategyTypes StrategyId { get; set; }

        /// <summary>
        /// Start date of strategy tracking
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// The name of the strategy
        /// </summary>
        public string Name { get; set; }
    }
}
