using System;
using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Widget that is used to publish portfolio in Finance\Pubs section
    /// to display portfolio on a publisher's website.
    /// </summary>
    public class PtWidgetContract
    {
        /// <summary>
        /// ID of the widget.
        /// </summary>
        public int WidgetId { get; set; }

        /// <summary>
        /// GUID identifier of the widget.
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// The name of the widget.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// List of trusted domains.
        /// </summary>
        public List<string> Domains { get; set; }

        /// <summary>
        /// The date with the time in UTC timezone when the widget was edited last time.
        /// </summary>
        public DateTime UpdateDateUtc { get; set; }

        /// <summary>
        /// The way to display a widget on publisher's website (Iframe, EmbedHtml)
        /// </summary>
        public PortfolioWidgetTypes WidgetType { get; set; }

        /// <summary>
        /// Style: Header row background.
        /// </summary>
        public string HeaderRowBackground { get; set; }

        /// <summary>
        /// Style: Font family.
        /// </summary>
        public string FontFamily { get; set; }

        /// <summary>
        /// Style: Font size.
        /// </summary>
        public int? FontSize { get; set; }

        /// <summary>
        /// Style: Container size width in pixels.
        /// </summary>
        public int? ContainerSizeWidth { get; set; }

        /// <summary>
        /// Style: Container size height in pixels.
        /// </summary>
        public int? ContainerSizeHeight { get; set; }

        /// <summary>
        /// Indicates whether widget is published or not.
        /// </summary>
        public bool IsPublished { get; set; }

        /// <summary>
        /// ID of the portfolio that is displayed in widget.
        /// </summary>
        public int PortfolioId { get; set; }
    }
}
