using System;
using System.Collections.Generic;
using System.Text;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to get VQ values
    /// </summary>
    public class GetVqValuesContract
    {
        /// <summary>
        /// List of Symbol IDs
        /// </summary>
        public List<int> SymbolIds { get; set; }

        /// <summary>
        /// First valid date to load VQ
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Last valid date to load VQ
        /// </summary>
        public DateTime ToDate { get; set; }
    }
}
