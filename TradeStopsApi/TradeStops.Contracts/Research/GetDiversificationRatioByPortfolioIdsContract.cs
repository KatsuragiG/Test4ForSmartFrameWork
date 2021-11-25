using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters of portfolios' diversification ratio calculation
    /// </summary>
    public class GetDiversificationRatioByPortfolioIdsContract
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
