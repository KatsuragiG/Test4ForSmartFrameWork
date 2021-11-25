using System.Collections.Generic;
using TradeStops.Common.Enums;
using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to edit a widget that is used to publish portfolio in Finance\Pubs section
    /// to display portfolio on a publisher's website.
    /// </summary>
    public class EditPtWidgetContract
    {
        /// <summary>
        /// The name of the widget.
        /// </summary>
        public Optional<string> Name { get; set; }

        /// <summary>
        /// List of trusted domains.
        /// </summary>
        public Optional<List<string>> Domains { get; set; }

        /// <summary>
        /// The way to display a widget on publisher's website (Iframe, EmbedHtml)
        /// </summary>
        public Optional<PortfolioWidgetTypes> Format { get; set; }

        /// <summary>
        /// Style: Header row background.
        /// </summary>
        public Optional<string> HeaderRowBackground { get; set; }

        /// <summary>
        /// Style: Font family.
        /// </summary>
        public Optional<string> FontFamily { get; set; }

        /// <summary>
        /// Style: Font size.
        /// </summary>
        public Optional<int> FontSize { get; set; }

        /// <summary>
        /// Style: Container size width in pixels.
        /// </summary>
        public Optional<int?> ContainerSizeWidth { get; set; }

        /// <summary>
        /// Style: Container size height in pixels.
        /// </summary>
        public Optional<int?> ContainerSizeHeight { get; set; }

        /// <summary>
        /// Indicates whether widget is published or not.
        /// </summary>
        public Optional<bool> IsPublished { get; set; }
    }
}
