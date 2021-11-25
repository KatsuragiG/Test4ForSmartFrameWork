using System;
using System.Collections.Generic;
using System.Text;

namespace TradeStops.Contracts
{
    /// <summary>
    /// The contract for receiving latest intraday ticks data aggregated by 1 min for a list of symbols.
    /// </summary>
    public class GetLastIntradayPricesAggregatedDataForSymbolsContract
    {
        /// <summary>
        /// List of SymbolIds to get latest intraday ticks data
        /// </summary>
        public List<int> SymbolIds { get; set; }
    }
}
