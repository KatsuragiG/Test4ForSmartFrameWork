using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Recently added newsletter position contract
    /// </summary>
    public class RecentlyAddedNewsletterPositionContract
    {
        /// <summary>
        /// ID of the newsletter portfolio
        /// </summary>
        public int PortfolioId { get; set; }

        /// <summary>
        /// Publisher type
        /// </summary>
        public PublisherTypes PublisherType { get; set; }

        /// <summary>
        /// Publisher name
        /// </summary>
        public string PublisherName { get; set; }

        /// <summary>
        /// Position reference date
        /// </summary>
        public DateTime? RefDate { get; set; }

        /// <summary>
        /// Symbol contract
        /// </summary>
        public SymbolContract Symbol { get; set; }

        /// <summary>
        /// Current SSI state
        /// </summary>
        public SsiCurrentValueContract SsiCurrentValue { get; set; }

        /// <summary>
        /// Currency values.
        /// </summary>
        public CurrencyContract Currency { get; set; }
    }
}
