using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Information about TradeSmith product
    /// </summary>
    public class ProductContract
    {
        /// <summary>
        /// ID of the product
        /// </summary>
        public Products ProductId { get; set; }

        /// <summary>
        /// Name of the product
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Obsolete (2020-05-12)! Landing page of the product. Can be null.
        /// <example>https://tradestops.com/</example>
        /// </summary>
        [Obsolete("2020-05-12. Use GetAllProductUrls endpoint to get urls.")]
        public string LandingPageUrl { get; set; }

        /// <summary>
        /// Obsolete (2020-05-12)! Login page of the product. Can be null.
        /// <example>https://cbe.tradestops.com/spa/login</example>
        /// </summary>
        [Obsolete("2020-05-12. Use GetAllProductUrls endpoint to get urls.")]
        public string LoginPageUrl { get; set; }
    }
}
