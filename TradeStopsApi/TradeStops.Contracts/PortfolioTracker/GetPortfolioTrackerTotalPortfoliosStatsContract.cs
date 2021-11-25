using System.Collections.Generic;

using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to get total portfolio statistics for requested portfolios calculated in DefaultCurrencyId
    /// </summary>
    public class GetPortfolioTrackerTotalPortfoliosStatsContract
    {
        /// <summary>
        /// Ids of portfolios that will be used to calculate total statistics.
        /// </summary>
        public List<int> PortfolioIds { get; set; }

        /// <summary>
        /// Types of total portfolio statistics to return.
        /// </summary>
        public List<PortfolioStatsTypes> PortfolioStatsTypes { get; set; }

        /// <summary>
        /// Currency Id in which all totals will be calculated.
        /// </summary>
        public int DefaultCurrencyId { get; set; }
    }
}
