using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Portfolio view.
    /// It is used in Finance website in Pubs section to publish portfolio and in My Gurus section to display it.
    /// </summary>
    public class PtPortfolioViewContract
    {
        // commented because field seems to be unnecessary outside of API,
        // and contract is shared between PT2 Publish and Finance Gurus where identifiers are different
        /////// <summary>
        /////// ID of the portfolio that will be displayed in My Gurus section in Finance website
        /////// with the columns from this view.
        /////// ViewColumns for ViewType = PortfolioTrackerPublish are used for this kind of views.
        /////// Primary key for PortfolioView because relationship between Portfolio and PortfolioView is 1-0..1
        /////// </summary>
        ////public int PortfolioId { get; set; }

        /// <summary>
        /// Column that is used to sort results.
        /// </summary>
        public ViewColumnTypes SortColumn { get; set; }

        /// <summary>
        /// Type of the sorting direction for the column that is used to sort (Ascending, Descending).
        /// </summary>
        public SortTypes SortType { get; set; }

        /// <summary>
        /// List of the columns for the view.
        /// </summary>
        public List<PtPortfolioViewColumnContract> Columns { get; set; }
    }
}
