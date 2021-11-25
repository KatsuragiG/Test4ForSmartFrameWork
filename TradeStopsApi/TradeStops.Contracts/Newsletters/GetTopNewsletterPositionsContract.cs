using System.Collections.Generic;
using TradeStops.Common.DataStructures;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to get newsletter portfolios vq allocation
    /// </summary>
    public class GetTopNewsletterPositionsContract
    {
        /// <summary>
        /// List of newsletter portfolios
        /// </summary>
        public List<NewslettersPortfolioKey> PortfolioIds { get; set; }

        /// <summary>
        /// Maximum number of newsletter positions to return.
        /// </summary>
        public int MaximumNumberOfPositions { get; set; }

        /// <summary>
        /// Use values adjusted by dividends during calculations Recommended value = True.
        /// </summary>
        public bool AdjustByDividends { get; set; }

        /// <summary>
        ///  Order by field.
        /// </summary>
        public NewsletterPositionOrderByFields OrderByField { get; set; }

        /// <summary>
        ///  Order type.
        /// </summary>
        public OrderTypes OrderType { get; set; }

        /// <summary>
        /// Default currency that will be used to convert positions in different currency.
        /// </summary>
        public int DefaultCurrencyId { get; set; }
    }
}
