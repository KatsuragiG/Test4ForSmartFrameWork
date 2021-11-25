using System;
using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to search symbols
    /// </summary>
    public class SearchSymbolsContract
    {
        /// <summary>
        /// String for search.
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// Maximal number of symbols.
        /// </summary>
        public int MaxResults { get; set; }

        /// <summary>
        /// (optional) Comma-separated values for select exchange in search
        /// </summary>
        public ExchangesFlags? ExchangesFlags { get; set; }

        /// <summary>
        /// (optional) Return only ETFs and other Funds.
        /// </summary>
        [Obsolete("2020-10-26. Pass SymbolTypes.Fund in SymbolTypes list instead.")]
        public bool FundsOnly { get; set; }

        /// <summary>
        /// List of symbol types to search for
        /// </summary>
        public List<SymbolTypes> SymbolTypes { get; set; }
    }
}
