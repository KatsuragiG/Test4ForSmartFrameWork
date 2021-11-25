using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// View contract.
    /// </summary>
    public class PublishersViewContract
    {
        /// <summary>
        /// View Id.
        /// </summary>
        public int ViewId { get; set; }

        /// <summary>
        /// Portfolio Id.
        /// </summary>
        public int PortfolioId { get; set; }

        /// <summary>
        /// Width.
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// WidthPreview.
        /// </summary>
        public int? WidthPreview { get; set; }

        /// <summary>
        /// Height.
        /// </summary>
        public int? Height { get; set; }

        /// <summary>
        /// View name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Indicates that the current Portfolio Name is displayed on the View.
        /// </summary>
        public bool IsShowName { get; set; }

        /// <summary>
        /// Indicates that the Print icon is displayed on the View.
        /// </summary>
        public bool IsShowPrint { get; set; }

        /// <summary>
        /// View Template Id. According to the selected template the View will show appropriate columns.
        /// </summary>
        public int? ViewTemplateId { get; set; }

        /// <summary>
        /// Defines how dates is displayed on the View.
        /// </summary>
        public PublishersDateFormatTypes DateFormat { get; set; }

        /// <summary>
        /// Defines what color and style are used for the View.
        /// </summary>
        public PublishersThemeTypes Theme { get; set; }

        /// <summary>
        /// Last update position.
        /// </summary>
        public PublishersViewPositionTypes? LastUpdatePosition { get; set; }

        /// <summary>
        /// The precision of all fields with Money format.
        /// </summary>
        public int MoneyPrecision { get; set; }

        /// <summary>
        /// The precision of all fields with Percent format.
        /// </summary>
        public int PercentPrecision { get; set; }

        /// <summary>
        /// View type.
        /// </summary>
        public PublishersViewTypes ViewType { get; set; }
    }
}
