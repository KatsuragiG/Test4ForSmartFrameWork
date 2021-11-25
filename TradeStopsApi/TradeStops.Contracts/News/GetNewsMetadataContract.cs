using System;
using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to get news metadata (preview info)
    /// </summary>
    public class GetNewsMetadataContract
    {
        /// <summary>
        /// (optional) List of Symbol IDs to search news
        /// </summary>
        public List<int> SymbolIds { get; set; }

        /// <summary>
        /// (optional) Pagination: ID of the last loaded news that user seen on the screen.
        /// This ID is used to avoid duplication of news when loading data for infinite scroll
        /// Currently it's guaranteed that News with minimal NewsId is the same as News with earlier creation date,
        /// so it's okay to use minimal NewsId from previously lodaded bulk of news. 
        /// </summary>
        public int? LastLoadedNewsId { get; set; }

        /// <summary>
        /// Pagination: Number of items to take (page size)
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// (optional) Start date for displaying news, including specified date.
        /// Filtering is carried out by the date of publication of the news.
        /// </summary>
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// (optional) End date for displaying news, including specified date.
        /// Filtering is carried out by the date of publication of the news.
        /// </summary>
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// (optional) Query to find substring in news title
        /// </summary>
        public string SearchQuery { get; set; }

        /// <summary>
        /// (optional) Indicates whether the news are hidden or visible
        /// </summary>
        public bool? IsVisible { get; set; }

        /// <summary>
        /// (optional) Return news only matching specified sources
        /// </summary>
        public List<int> NewsSourceIds { get; set; }
    }
}
