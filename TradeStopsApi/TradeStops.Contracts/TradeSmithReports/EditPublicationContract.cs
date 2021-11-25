using System;
using System.Collections.Generic;
using TradeStops.Common.Enums;
using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to edit publication
    /// </summary>
    public class EditPublicationContract
    {
        /// <summary>
        /// (optional) The publication type
        /// </summary>
        public Optional<PublicationTypes> PublicationType { get; set; }

        /// <summary>
        /// (optional) The title of publication
        /// </summary>
        public Optional<string> Title { get; set; }

        /// <summary>
        /// (optional) The description of publication
        /// </summary>
        public Optional<string> Description { get; set; }

        /// <summary>
        /// (optional) The url for publication image
        /// </summary>
        public Optional<string> ImageUrl { get; set; }

        /// <summary>
        /// (optional) The url for publication full size image
        /// </summary>
        public Optional<string> ImageFullSizeUrl { get; set; }

        /// <summary>
        /// (optional) The source url for publication
        /// </summary>
        public Optional<string> SourceUrl { get; set; }

        /// <summary>
        /// (optional) The date and time of publication in utc
        /// </summary>
        public Optional<DateTime> PublicationDate { get; set; }

        /// <summary>
        /// (optional) The Name of published pdf report
        /// </summary>
        public Optional<string> PdfReportName { get; set; }

        /// <summary>
        /// (optional) The image name for publication
        /// </summary>
        public Optional<string> ImageName { get; set; }

        /// <summary>
        /// (optional) The source type of publication
        /// </summary>
        public Optional<PublicationSources> PublicationSource { get; set; }

        /// <summary>
        /// (optional) Author of the publication
        /// </summary>
        public Optional<string> Author { get; set; }

        /// <summary>
        /// (optional) List of publication category IDs
        /// </summary>
        public Optional<List<int>> PublicationCategoryIds { get; set; }
    }
}
