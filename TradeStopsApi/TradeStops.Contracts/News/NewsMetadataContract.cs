using System;
using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Short information about news article to display in the list of news
    /// </summary>
    public class NewsMetadataContract
    {
        /// <summary>
        /// ID of the news
        /// </summary>
        public int NewsId { get; set; }
         
        ////public int VendorId { get; set; }
        ////public int SourceId { get; set; }
        
        /////// <summary>
        /////// Author. Can be null
        /////// </summary>
        ////public string Author { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Description. Can be null
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// URL of the original article. Can be null
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Url to original image. Can be null or empty
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Url to resized image. Can be null or empty
        /// </summary>
        public string Image110X110Url { get; set; }

        /// <summary>
        /// Url to resized image. Can be null or empty
        /// </summary>
        public string Image600X400Url { get; set; }

        /// <summary>
        /// Publication date in UTC
        /// </summary>
        public DateTime PublicationDate { get; set; }

        /// <summary>
        /// Date in UTC when news were imported to our database
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Date of last update for news. Can be null
        /// </summary>
        public DateTime UpdateDate { get; set; }

        ////public bool IsProcessed { get; set; } // not null

        /// <summary>
        /// News source name
        /// </summary>
        public string SourceName { get; set; }

        /////// <summary>
        /////// Url to the main page of the news source
        /////// </summary>
        ////public string SourceUrl { get; set; }

        /// <summary>
        /// Indicates whether the news are hidden or visible
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Indicates whether the news are under public license.
        /// </summary>
        public bool Licensed { get; set; }

        /// <summary>
        /// Information about symbols related to news
        /// </summary>
        public List<NewsSymbolContract> RelatedSymbols { get; set; }
    }
}
