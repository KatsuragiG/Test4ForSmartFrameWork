using System.Collections.Generic;
using TradeStops.Common.DataStructures;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to get newsletter portfolios asset allocation
    /// </summary>
    public class GetNewsletterPortfoliosAssetAllocationContract
    {
        /// <summary>
        /// List of newsletter portfolios
        /// </summary>
        public List<NewslettersPortfolioKey> PortfolioIds { get; set; }
    }
}
