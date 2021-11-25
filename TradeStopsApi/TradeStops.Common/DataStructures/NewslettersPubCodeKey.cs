using System;
using TradeStops.Common.Enums;
using TradeStops.Common.Extensions;

namespace TradeStops.Common.DataStructures
{
    /// <summary>
    /// Composite key that represents unique identifier of newsletters pub-code that we have in remote CRM.
    /// </summary>
    public class NewslettersPubCodeKey : IEquatable<NewslettersPubCodeKey>
    {
        /// <summary>
        /// Parameterless constructor.
        /// Can be necessary for serialization purposes.
        /// </summary>
        [Obsolete("Use parameterized constructor instead", error: true)]
        public NewslettersPubCodeKey()
        {
        }

        /// <summary>
        /// Parameterized constructor.
        /// </summary>
        /// <param name="pubCode">PubCode from CRM.</param>
        /// <param name="subtype">Subtype for PubCode. Can be null.</param>
        /// <param name="subscriptionSourceId">The source of subscription. It means the CRM where corresponding PubCode exist.</param>
        public NewslettersPubCodeKey(string pubCode, string subtype, NewslettersSubscriptionSources subscriptionSourceId)
        {
            PubCode = pubCode;
            Subtype = string.IsNullOrWhiteSpace(subtype) ? null : subtype;
            SubscriptionSourceId = subscriptionSourceId;
        }

        /// <summary>
        /// PubCode is unique in the CRM, but different CRMs can have the same pub-codes (for example TOT). 
        /// </summary>
        public string PubCode { get; set; }

        /// <summary>
        /// Subtype is used to separate subscriptions assigned to different pub-codes
        /// </summary>
        public string Subtype { get; set; }

        /// <summary>
        /// Indicates the Synchronization Source (CRM) of portfolio pub-codes.
        /// In other words, it's the CRM where these pub-codes are defined.
        /// </summary>
        public NewslettersSubscriptionSources SubscriptionSourceId { get; set; }

        public bool Equals(NewslettersPubCodeKey other)
        {
            if (other == null)
            {
                return false;
            }

            return PubCode.EqualsIgnoreCase(other.PubCode) && Subtype.EqualsIgnoreCase(other.Subtype) && SubscriptionSourceId == other.SubscriptionSourceId;
        }

        public override bool Equals(object obj)
        {
            var other = obj as NewslettersPubCodeKey;

            return Equals(other);
        }

        public override int GetHashCode()
        {
            return Tuple.Create(PubCode.ToUpper(), Subtype, SubscriptionSourceId).GetHashCode();
        }
    }
}
