using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Asset allocation symbol weight
    /// </summary>
    public class AssetAllocationSymbolWeightContract
    {
        /// <summary>
        /// Symbol
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Percent of the asset weight by symbol of the portfolio value.
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// All position Ids related to this symbol.
        /// </summary>
        public List<int> RelatedPositionIds { get; set; }
    }
}