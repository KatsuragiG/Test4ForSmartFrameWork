using TradeStops.Common.Enums;
using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Contract with view template fields to edit.
    /// </summary>
    public class EditPublishersViewTemplateContract
    {
        /// <summary>
        /// (optional) View template name.
        /// </summary>
        public Optional<string> Name { get; set; }

        /// <summary>
        /// (optional) Defines what positions are displayed on the View.
        /// </summary>
        public Optional<PublishersPositionShowTypes> PositionShowType { get; set; }

        /// <summary>
        /// (optional) Indicates that the Portfolio Stats is displayed on the View.
        /// </summary>
        public Optional<bool> ShowPortfolioStats { get; set; }

        /// <summary>
        /// (optional) Indicates that positions are displayed as separated by trade groups.
        /// </summary>
        public Optional<bool> GroupTrades { get; set; }

        /// <summary>
        /// (optional) Indicates that trade groups are displayed collapsed on the View.
        /// </summary>
        public Optional<bool> CollapsedGroups { get; set; }

        /// <summary>
        /// (optional) Indicates that symbol chart popup window is diplayed on the View.
        /// </summary>
        public Optional<bool> SymbolClickCharts { get; set; }

        /// <summary>
        /// (optional) Order direction.
        /// </summary>
        public Optional<OrderTypes>  SortOrder { get; set; }

        /// <summary>
        /// (optional) Defines view column that is used to determine the sorting order.
        /// </summary>
        public Optional<PublishersColumnTypes> SortColumn { get; set; }

        /// <summary>
        /// (optional) Defines view column that is automatically expanded in such way 
        /// that the width of all other columns of the view template won't be less 
        /// than specified for them.
        /// </summary>
        public Optional<PublishersColumnTypes> AutoExpand { get; set; }

        /// <summary>
        /// (optional) Indicates that 'Other' column is displayed on the View.
        /// </summary>
        public Optional<bool> ShowOther { get; set; }
    }
}
