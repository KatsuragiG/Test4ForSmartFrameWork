using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Information about news source
    /// </summary>
    public class NewsSourceContract
    {
        /// <summary>
        /// News source ID
        /// </summary>
        public int NewsSourceId { get; set; }

        /// <summary>
        /// Source name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Source desription
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// URL of the original source.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Source category
        /// </summary>
        public NewsCategories Category { get; set; }
        ////public int LanguageId { get; set; }
        ////public int CountryId { get; set; } // not the same id as in TradeStops3..Countries

        /// <summary>
        /// Indicates whether the news are under public license.
        /// </summary>
        public bool Licensed { get; set; }

        /// <summary>
        /// URL to the icon of the original source.
        /// </summary>
        public string IconUrl { get; set; }
    }
}
