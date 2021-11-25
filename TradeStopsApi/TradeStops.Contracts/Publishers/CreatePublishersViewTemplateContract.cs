using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create view template.
    /// </summary>
    public class CreatePublishersViewTemplateContract
    {
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
    }
}
