using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// View template contract.
    /// </summary>
    public class PublishersViewTemplateContract
    {
        /// <summary>
        /// View template Id.
        /// </summary>
        public int ViewTemplateId { get; set; }

        /// <summary>
        /// View template type.
        /// </summary>
        public PublishersViewTemplateTypes ViewTemplateType { get; set; }

        /// <summary>
        /// Customer Id.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// View template name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Defines what positions are displayed on the View.
        /// </summary>
        public PublishersPositionShowTypes PositionShowType { get; set; }

        /// <summary>
        /// Indicates that the Portfolio Stats is displayed on the View.
        /// </summary>
        public bool ShowPortfolioStats { get; set; }

        /// <summary>
        /// Indicates that positions are displayed as separated by trade groups.
        /// </summary>
        public bool GroupTrades { get; set; }

        /// <summary>
        /// Indicates that trade groups are displayed collapsed on the View.
        /// </summary>
        public bool CollapsedGroups { get; set; }

        /// <summary>
        /// Indicates that symbol chart popup window is diplayed on the View.
        /// </summary>
        public bool SymbolClickCharts { get; set; }

        /// <summary>
        /// Indicates that 'Other' column is displayed on the View.
        /// </summary>
        public bool ShowOther { get; set; }

        /// <summary>
        /// Order direction.
        /// </summary>
        public OrderTypes SortOrder { get; set; }

        /// <summary>
        /// Defines view column that is used to determine the sorting order.
        /// </summary>
        public PublishersColumnTypes? SortColumn { get; set; }

        /// <summary>
        /// Defines view column that is automatically expanded in such way 
        /// that the width of all other columns of the view template won't be less 
        /// than specified for them.
        /// </summary>
        public PublishersColumnTypes? AutoExpand { get; set; }
    }
}
