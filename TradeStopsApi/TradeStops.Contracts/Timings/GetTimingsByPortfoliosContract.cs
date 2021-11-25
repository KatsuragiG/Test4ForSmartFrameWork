using System.Collections.Generic;
using TradeStops.Common.DataStructures;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to get timings
    /// </summary>
    public class GetTimingsByPortfoliosContract
    {
        /// <summary>
        /// (optional) IDs of portfolios to get symbol IDs.
        /// Increases the number of symbols to get timings.
        /// </summary>
        public List<int> PortfolioIds { get; set; }

        /// <summary>
        /// (optional) Newsletter portfolio keys to get symbol IDs.
        /// Increases the number of symbols to get timings.
        /// </summary>
        public List<NewslettersPortfolioKey> NewsletterPortfolioKeys { get; set; }

        /// <summary>
        /// (optional) Baskets to get symbol IDs.
        /// Increases the number of symbols to get timings.
        /// </summary>
        public List<int> BasketIds { get; set; }

        /// <summary>
        /// (optional) Ssi statuses of symbols to get timings.
        /// Reduces the number of symbols received by PortfolioIds, NewsletterPortfolioKeys, MarketOutlookGroupIds fields.
        /// </summary>
        public List<SsiStatuses> SsiStatuses { get; set; }

        /// <summary>
        /// (optional) Symbol types to get timings.
        /// Reduces the number of received timings.
        /// </summary>
        public List<SymbolTypes> SymbolTypes { get; set; }
    }
}
