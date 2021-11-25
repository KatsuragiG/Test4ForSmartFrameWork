using TradeStops.Common.Enums;
using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Contract with view fields to edit.
    /// </summary>
    public class EditPublishersViewContract
    {
        /// <summary>
        /// (optional) View name.
        /// </summary>
        public Optional<string> Name { get; set; }

        /// <summary>
        /// (optional) View Template Id. According to the selected Template the View will show appropriate columns.
        /// </summary>
        public Optional<int> ViewTemplateId { get; set; }

        /// <summary>s
        /// (optional) Defines what color and style are used for the View.
        /// </summary>
        public Optional<PublishersThemeTypes> Theme { get; set; }

        /// <summary>
        /// (optional) Width.
        /// </summary>
        public Optional<int> Width { get; set; }

        /// <summary>
        /// (optional) Width preview.
        /// </summary>
        public Optional<int> WidthPreview { get; set; }

        /// <summary>
        /// (optional) Height.
        /// </summary>
        public Optional<int> Height { get; set; }

        /// <summary>
        /// (optional) Indicates that the current Portfolio Name is displayed on the View.
        /// </summary>
        public Optional<bool> IsShowName { get; set; }

        /// <summary>
        /// (optional) Indicates that the Print icon is displayed on the View.
        /// </summary>
        public Optional<bool> IsShowPrint { get; set; }

        /// <summary>
        /// (optional) Last update position.
        /// </summary>
        public Optional<PublishersViewPositionTypes?> LastUpdatePosition { get; set; }

        /// <summary>
        /// (optional) Defines how dates is displayed on the View.
        /// </summary>
        public Optional<PublishersDateFormatTypes> DateFormat { get; set; }

        /// <summary>
        /// (optional) The precision of all fields with Money format.
        /// </summary>
        public Optional<int> MoneyPrecision { get; set; }

        /// <summary>
        /// (optional) The precision of all fields with Percent format.
        /// </summary>
        public Optional<int> PercentPrecision { get; set; }
    }
}
