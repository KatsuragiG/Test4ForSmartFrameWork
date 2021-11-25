using System.Collections.Generic;
using TradeStops.Common.DataStructures;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to get newsletter portfolios global rank allocation
    /// </summary>
    public class GetNewsletterPortfoliosGlobalRankAllocationContract
    {
        /// <summary>
        /// List of newsletter portfolios
        /// </summary>
        public List<NewslettersPortfolioKey> PortfolioIds { get; set; }
    }
}
