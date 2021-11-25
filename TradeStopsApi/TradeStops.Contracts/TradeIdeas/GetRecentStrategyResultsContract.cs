using System;
using System.Collections.Generic;
using TradeStops.Common.Enums;
using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Contract to get Symbols that were recently added to strategies
    /// </summary>
    public class GetRecentStrategyResultsContract
    {
        /// <summary>
        /// List of StrategyTypes from which symbols will be searched
        /// </summary>
        public List<InvestmentStrategyTypes> StrategyTypes { get; set; }

        /// <summary>
        /// Get data added from a given date.
        /// </summary>
        public DateTime StartDate { get; set;  }

        /// <summary>
        /// Get data added to a given date.
        /// </summary>
        public DateTime FinishDate { get; set; }
    }
}
