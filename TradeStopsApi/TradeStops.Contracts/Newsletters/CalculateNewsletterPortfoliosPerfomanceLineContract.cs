using System;
using System.Collections.Generic;
using System.Text;
using TradeStops.Common.DataStructures;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters of newsletter portfolios performance line calculation
    /// </summary>
    public class CalculateNewsletterPortfoliosPerfomanceLineContract
    {
        /// <summary>
        /// List of newsletter portfolios
        /// </summary>
        public List<NewslettersPortfolioKey> PortfolioIds { get; set; }

        /// <summary>
        /// Start date
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Finish date
        /// </summary>
        public DateTime ToDate { get; set; }

        /// <summary>
        /// Currency ID. All prices will be converted to this currency during calculation
        /// </summary>
        public int CurrencyId { get; set; }
    }
}
