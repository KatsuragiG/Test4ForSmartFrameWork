using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Market allocation for newsletter portfolios
    /// </summary>
    public class NewsletterMarketAllocationContract
    {
        /// <summary>
        /// List of allocated markets
        /// </summary>
        public List<NewsletterMarketAllocationGroupContract> Groups { get; set; }        
    }
}
