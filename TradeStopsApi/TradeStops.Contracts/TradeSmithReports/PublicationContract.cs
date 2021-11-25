using System;
using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Publication
    /// </summary>
    public class PublicationContract
    {
        /// <summary>
        /// ID of the publication
        /// </summary>
        public int PublicationId { get; set; }

        /// <summary>
        /// The publication type
        /// </summary>
        public PublicationTypes PublicationType { get; set; }

        /// <summary>
        /// The title of publication
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The description of publication
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The pdf report name of publication
        /// </summary>
        public string PdfReportName { get; set; }

        /// <summary>
        /// The image name of publication
        /// </summary>
        public string ImageName { get; set; }

        /// <summary>
        /// The date and time of publication
        /// </summary>
        public DateTime PublicationDate { get; set; }

        /// <summary>
        /// The url for publication image
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// The url for publication full size image
        /// </summary>
        public string ImageFullSizeUrl { get; set; }

        /// <summary>
        /// The source url for publication
        /// </summary>
        public string SourceUrl { get; set; }

        /// <summary>
        /// The source type of publication
        /// </summary>
        public PublicationSources PublicationSource { get; set; }

        /// <summary>
        /// The html of publication
        /// </summary>
        public string HtmlReport { get; set; }

        /// <summary>
        /// List of publication categories
        /// </summary>
        public List<string> PublicationCategories { get; set; }

        /// <summary>
        /// Publication author
        /// </summary>
        public string Author { get; set; }
    }
}
