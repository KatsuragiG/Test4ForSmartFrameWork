using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Params to get Smith Rank.
    /// </summary>
    public class GetSmithRankContract
    {
        /// <summary>
        /// List of symbol IDs.
        /// </summary>
        public List<int> SymbolIds { get; set; }

        /// <summary>
        /// Determines if the prices adjusted by dividends will be used in calculations.
        /// </summary>
        public bool AdjustByDividends { get; set; }
    }
}
