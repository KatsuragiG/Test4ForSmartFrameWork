using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to get portfolio statistics
    /// </summary>
    public class GetPortfolioStatsContract
    {
        /// <summary>
        /// IDs of portfolios
        /// </summary>
        public List<int> PortfolioIds { get; set; }

        /// <summary>
        /// Currency ID for statistics
        /// </summary>
        public int DefaultCurrencyId { get; set; }
    }
}
