using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Contract with all necessary fields to publish portfolio.
    /// </summary>
    public class PublishPtPortfolioContract
    {
        /// <summary>
        /// ID of the Portfolio to publish
        /// </summary>
        public int PortfolioId { get; set; }

        /// <summary>
        /// Name of the portfolio that is going to be published.
        /// </summary>
        public string PortfolioName { get; set; }

        /// <summary>
        /// List of pub-codes to assign to published portfolio.
        /// </summary>
        public List<string> PortfolioPubCodes { get; set; }

        /// <summary>
        /// Indicates whether Portfolio must be published (true) or unpublished (false)
        /// </summary>
        public bool IsPortfolioPublished { get; set; }

        /// <summary>
        /// Positions from the portfolio that must be published/unpublished.
        /// </summary>
        public List<PublishPtPositionContract> Positions { get; set; }

        /// <summary>
        /// Column that is used for sorting for published portfolio, when it's displayed in My Gurus.
        /// </summary>
        public ViewColumnTypes ViewSortColumn { get; set; }

        /// <summary>
        /// Sorting direction for the ViewSortColumn.
        /// </summary>
        public SortTypes ViewSortType { get; set; }

        /// <summary>
        /// Columns that will be used to display portfolio in My Gurus section.
        /// </summary>
        public List<PtPortfolioViewColumnContract> ViewColumns { get; set; }
    }
}
