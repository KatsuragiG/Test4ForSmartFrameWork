using System;
using System.Collections.Generic;
using System.Text;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to get prices
    /// </summary>
    public class GetPricesContract
    {
        /// <summary>
        /// List of Symbol IDs
        /// </summary>
        public List<int> SymbolIds { get; set; }

        /// <summary>
        /// First valid date to load prices
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Last valid date to load prices
        /// </summary>
        public DateTime ToDate { get; set; }
    }
}
