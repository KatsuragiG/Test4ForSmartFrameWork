using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters required for Portfolios VQ calculation
    /// </summary>
    public class GetPortfoliosVqContract
    {
        /// <summary>
        /// Id of currency which will be used for calculation
        /// </summary>
        public int DefaultCurrencyId { get; set; }

        /// <summary>
        /// List of portfolio IDs
        /// </summary>
        public List<int> PortfolioIds { get; set; }
    }
}
