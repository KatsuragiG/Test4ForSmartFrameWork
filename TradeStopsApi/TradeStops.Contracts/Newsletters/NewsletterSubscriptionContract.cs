using System;
using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class NewsletterSubscriptionContract
    {
        public PublisherTypes PublisherType { get; set; }

        /// <summary>
        /// Internal column. Will be unnecessary when PublisherType will be passed as int enum. Will be removed in the middle of 2022.
        /// </summary>
        [Obsolete("Used by mobile app. Can be removed in the middle of 2022 when string enums will be serialized as int.")]
        public int PublisherId { get; set; }

        public string PublisherName { get; set; }

        public NewslettersSubscriptionSources PublishingSource { get; set; }

        public List<NewsletterPortfolioContract> AvailablePortfolios { get; set; }

        public List<NewsletterPortfolioContract> OtherPortfolios { get; set; }
    }
}
