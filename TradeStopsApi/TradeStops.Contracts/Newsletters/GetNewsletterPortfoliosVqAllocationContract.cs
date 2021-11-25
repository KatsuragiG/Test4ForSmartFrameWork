using System.Collections.Generic;
using TradeStops.Common.DataStructures;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to get newsletter portfolios vq allocation
    /// </summary>
    public class GetNewsletterPortfoliosVqAllocationContract
    {
        /// <summary>
        /// List of newsletter portfolios
        /// </summary>
        public List<NewslettersPortfolioKey> PortfolioIds { get; set; }

        /// <summary>
        /// Default currency that will be used to convert positions in different currency.
        /// </summary>
        public int DefaultCurrencyId { get; set; }
    }
}
