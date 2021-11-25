using System;
using System.Collections.Generic;
using TradeStops.Common.DataStructures;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Newsletter Portfolio
    /// </summary>
    public class NewsletterPortfolioContract
    {
        /// <summary>
        /// ID of the publisher.
        /// </summary>
        public PublisherTypes PublisherType { get; set; }

        /// <summary>
        /// The name of the publisher.
        /// </summary>
        public string PublisherName { get; set; } // consider to use value from NewsletterSubscriptionContract

        /// <summary>
        /// ID of the portfolio.
        /// Non-unique with portfolios for other publishers,
        /// but unique for portfolios from the same publisher.
        /// </summary>
        public int NewsletterPortfolioId { get; set; }

        /// <summary>
        /// The name of newsletter portfolio
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The list of keys, where each key is the set of PubCode with Subtype and corresponding SubscriptionSource.
        /// User must have any subscription with the corresponding key to get access to this portfolio.
        /// </summary>
        public List<NewslettersPubCodeKey> PubCodeKeys { get; set; }

        /// <summary>
        /// The source where we load symbols data for the portfolio.
        /// </summary>
        public NewslettersPublisherSources SourceType { get; set; }
    }
}
