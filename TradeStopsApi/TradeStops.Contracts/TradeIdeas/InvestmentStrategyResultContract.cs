using System;
using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Investment strategy result
    /// </summary>
    public class InvestmentStrategyResultContract
    {
        /// <summary>
        /// Symbol ID
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Symbol contract
        /// </summary>
        public SymbolContract Symbol { get; set; }

        /// <summary>
        /// Currency contract
        /// </summary>
        public CurrencyContract Currency { get; set; }

        /// <summary>
        /// Latest price contract
        /// </summary>
        public PriceContract LatestPrice { get; set; }

        /// <summary>
        /// Previous price contract
        /// </summary>
        public PriceContract PreviousPrice { get; set; }

        /// <summary>
        /// Current SSI value contract
        /// </summary>
        public SsiCurrentValueContract SsiCurrentValue { get; set; }

        /// <summary>
        /// List of recommended options for stock symbol.
        /// </summary>
        public List<RecommendedOptionContract> RecommendedOptions { get; set; }

        /// <summary>
        /// VQ value
        /// </summary>
        public decimal VqValue { get; set; }

        /// <summary>
        /// Average VQ value for 30 years
        /// </summary>
        public decimal Average30YearsVq { get; set; }

        /// <summary>
        /// VQ Ratio
        /// </summary>
        public decimal VqRatio { get; set; }

        /// <summary>
        /// Volume
        /// </summary>
        public long? Volume { get; set; }

        /// <summary>
        /// Price/Earnings ratio
        /// </summary>
        public decimal? PriceEarningsRatio { get; set; }

        /// <summary>
        /// Dividend yield
        /// </summary>
        public decimal? DividendYield { get; set; }

        /// <summary>
        /// Market cap
        /// </summary>
        public long? MarketCap { get; set; }

        /// <summary>
        /// One-year high value
        /// </summary>
        public decimal? OneYearHigh { get; set; }

        /// <summary>
        /// The name of the sector
        /// </summary>
        public string Sector { get; set; }

        /// <summary>
        /// Smith Rank
        /// </summary>
        public decimal SmithRank { get; set; }

        /// <summary>
        /// Symbol update time in the current strategy.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// List of newsletter portfolios that contain current symbol
        /// </summary>
        public List<NewslettersPortfolioWithSymbolRefDateContract> NewsletterPortfolios { get; set; }

        /// <summary>
        /// List of strategies that contain current symbol
        /// </summary>
        public List<InvestmentStrategyContract> Strategies { get; set; }
    }
}
