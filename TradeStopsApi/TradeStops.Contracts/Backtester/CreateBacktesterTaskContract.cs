using System;
using System.Collections.Generic;
using TradeStops.Common.DataStructures;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create a task for Backtester
    /// </summary>
    public class CreateBacktesterTaskContract
    {
        /// <summary>
        /// Start date of backtesting period.
        /// To store correctly, it must be just date without time and without timezone information.
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// End date of backtesting period. Only Date part is stored into database.
        /// To store correctly, it must be just date without time and without timezone information.
        /// </summary>
        public DateTime ToDate { get; set; }

        /// <summary>
        /// Individual securities that are going to be analyzed.
        /// </summary>
        public List<int> SymbolIds { get; set; }

        /// <summary>
        /// User's portfolios that are going to be analyzed.
        /// </summary>
        public List<int> UserPortfolioIds { get; set; }

        /// <summary>
        /// Newsletters Portfolios that are going to be analyzed
        /// </summary>
        public List<NewslettersPortfolioKey> NewslettersPortfolioKeys { get; set; }

        /// <summary>
        /// List of strategy IDs that are used to analyze
        /// </summary>
        public List<int> StrategyIds { get; set; }
    }
}
