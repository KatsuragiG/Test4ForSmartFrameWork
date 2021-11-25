using System.Collections.Generic;
using TradeStops.Common.DataStructures;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to get newsletter portfolios martket allocation
    /// </summary>
    public class GetNewsletterPortfoliosMarketAllocationContract
    {
        /// <summary>
        /// List of newsletter portfolios
        /// </summary>
        public List<NewslettersPortfolioKey> PortfolioIds { get; set; }

        /// <summary>
        /// List of symbol groups (markets) to calculate allocation
        /// </summary>
        public List<SymbolGroupTypes> SymbolGroupIds { get; set; }
    }
}
