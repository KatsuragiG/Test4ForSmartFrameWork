using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create a view for portfolio that will be used to display published portfolio
    /// in My Gurus section in Finance website.
    /// </summary>
    public class CreatePtPortfolioViewContract
    {
        /// <summary>
        /// ID of the portfolio that will be displayed in My Gurus section in Finance website
        /// with the columns from this view.
        /// ViewColumns for ViewType = PortfolioTrackerPublish are used for this kind of views.
        /// </summary>
        public int PortfolioId { get; set; }

        /// <summary>
        /// Column that is used to sort results.
        /// </summary>
        public ViewColumnTypes SortColumn { get; set; }

        /// <summary>
        /// Type of the sorting direction for the column that is used to sort (Ascending, Descending).
        /// </summary>
        public SortTypes SortType { get; set; }
    }
}
