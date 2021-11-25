using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// URLs for Product.
    /// </summary>
    public class ProductUrlsContract
    {
        /// <summary>
        /// ID of the product.
        /// </summary>
        public Products ProductId { get; set; }

        /// <summary>
        /// Landing page of the product. Can be null.
        /// <example>https://tradestops.com/</example>
        /// </summary>
        public string LandingPageUrl { get; set; }

        /// <summary>
        /// Root URL of the web application. Can be null.
        /// <example>https://cbe.tradestops.com/</example>
        /// </summary>
        public string ApplicationRootUrl { get; set; }

        /// <summary>
        /// Login page of the product. Can be null.
        /// <example>https://cbe.tradestops.com/spa/login</example>
        /// </summary>
        public string LoginPageUrl { get; set; }
    }
}
