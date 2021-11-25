using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Contract for getting an value allocation grouped by symbols.
    /// </summary>
    public class GetValueAllocationContract
    {
        /// <summary>
        /// Portfolio Ids with positions
        /// </summary>
        public List<int> PortfolioIds { get; set; }

        /// <summary>
        /// The currency id to which the value of all positions will be converted.
        /// </summary>
        public int DefaultCurrencyId { get; set; }
    }
}
