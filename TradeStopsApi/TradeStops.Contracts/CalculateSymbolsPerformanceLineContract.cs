using System;
using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters of symbols performance line calculation
    /// </summary>
    public class CalculateSymbolsPerformanceLineContract
    {
        /// <summary>
        /// Symbol IDs
        /// </summary>
        public List<int> SymbolIds { get; set; }

        /// <summary>
        /// Start date
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Finish date
        /// </summary>
        public DateTime ToDate { get; set; }

        /// <summary>
        /// Currency ID. All prices will be converted to this currency during calculation
        /// </summary>
        public int DefaultCurrencyId { get; set; }
    }
}
