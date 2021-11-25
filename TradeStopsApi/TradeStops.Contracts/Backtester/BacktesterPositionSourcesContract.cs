using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// The source of positions (symbols) to backtest
    /// </summary>
    public class BacktesterPositionSourcesContract
    {
        /// <summary>
        /// Newsletters
        /// </summary>
        public List<NewsletterPortfolioContract> Newsletters { get; set; }

        /// <summary>
        /// Portfolios
        /// </summary>
        public List<PortfolioContract> Portfolios { get; set; }

        /// <summary>
        /// Symbols
        /// </summary>
        public List<SymbolContract> Symbols { get; set; }
    }
}
