using System;
using TradeStops.Common.Enums;

namespace TradeStops.Common.DataStructures
{
    /// <summary>
    /// Composite key that represents unique identifier of portfolio from newsletters.
    /// </summary>
    public class NewslettersPortfolioKey : IEquatable<NewslettersPortfolioKey>
    {
        /// <summary>
        /// Parameterless constructor.
        /// Can be necessary for serialization purposes.
        /// </summary>
        [Obsolete("Use parameterized constructor instead", error: true)]
        public NewslettersPortfolioKey()
        {
        }

        /// <summary>
        /// Parameterized constructor.
        /// </summary>
        /// <param name="publisherType">ID of the publisher.</param>
        /// <param name="portfolioId">ID of the portfolio.</param>
        public NewslettersPortfolioKey(PublisherTypes publisherType, int portfolioId)
        {
            PublisherType = publisherType;
            PortfolioId = portfolioId;
        }

        /// <summary>
        /// ID of the newsletters publisher.
        /// </summary>
        public PublisherTypes PublisherType { get; set; } // todo: PublisherId is better name, but 'PublisherType' is used for consistency with other code

        /// <summary>
        /// ID of the newsletters portfolio.
        /// </summary>
        public int PortfolioId { get; set; }

        public bool Equals(NewslettersPortfolioKey other)
        {
            if (other == null)
            {
                return false;
            }

            return PublisherType == other.PublisherType && PortfolioId == other.PortfolioId;
        }

        public override bool Equals(object obj)
        {
            var other = obj as NewslettersPortfolioKey;

            return Equals(other);
        }

        public override int GetHashCode()
        {
            return Tuple.Create(PublisherType, PortfolioId).GetHashCode();
        }
    }
}
