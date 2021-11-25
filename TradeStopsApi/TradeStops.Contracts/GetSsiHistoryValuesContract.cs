using System;
using System.Collections.Generic;
using System.Text;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to get SSI values
    /// </summary>
    public class GetSsiHistoryValuesContract
    {
        /// <summary>
        /// List of Symbol IDs
        /// </summary>
        public List<int> SymbolIds { get; set; }

        /// <summary>
        /// First valid date to load SSI
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Last valid date to load SSI
        /// </summary>
        public DateTime ToDate { get; set; }
    }
}
