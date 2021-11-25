using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Newsletter portfolio with corresponding RefDate for Symbol
    /// </summary>
    public class NewslettersPortfolioWithSymbolRefDateContract
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public NewslettersPortfolioWithSymbolRefDateContract()
        {
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="portfolio">Portfolio</param>
        /// <param name="refDate">Reference date</param>
        public NewslettersPortfolioWithSymbolRefDateContract(NewsletterPortfolioContract portfolio, DateTime? refDate)
        {
            NewsletterPortfolio = portfolio;
            RefDate = refDate;
        }

        /// <summary>
        /// Reference date for Symbol
        /// </summary>
        public DateTime? RefDate { get; set; }

        /// <summary>
        /// Closest price to reference date
        /// </summary>
        public decimal? ClosePriceOnRefDate { get; set; }

        /// <summary>
        /// Newsletter portfolio
        /// </summary>
        public NewsletterPortfolioContract NewsletterPortfolio { get; set; }
    }
}
