using System;
using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create publication
    /// </summary>
    public class CreatePublicationContract
    {
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
        /// The url for publication image
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// The url for publication full size umage
        /// </summary>
        public string ImageFullSizeUrl { get; set; }

        /// <summary>
        /// The source url for publication
        /// </summary>
        public string SourceUrl { get; set; }

        /// <summary>
        /// The date and time of publication in utc
        /// </summary>
        public DateTime PublicationDate { get; set; }

        /// <summary>
        /// The Name of published pdf report
        /// </summary>
        public string PdfReportName { get; set; }

        /// <summary>
        /// The image name for publication
        /// </summary>
        public string ImageName { get; set; }

        /// <summary>
        /// The source type of publication
        /// </summary>
        public PublicationSources PublicationSource { get; set; }

        /// <summary>
        /// Author of the publication
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// List of publication category IDs
        /// </summary>
        public List<int> PublicationCategoryIds { get; set; }
    }
}
