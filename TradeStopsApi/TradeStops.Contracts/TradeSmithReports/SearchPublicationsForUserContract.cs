using System;
using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to get publications
    /// </summary>
    public class SearchPublicationsForUserContract
    {
        /// <summary>
        /// (Optional) Publication types
        /// </summary>
        public List<PublicationTypes> PublicationTypes { get; set; }

        /// <summary>
        /// Start date and time to search publications in utc
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// End date and time to search publications in utc
        /// </summary>
        public DateTime ToDate { get; set; }

        /// <summary>
        /// (Optional) Items to skip count
        /// </summary>
        public int? Start { get; set; }

        /// <summary>
        /// (Optional)  Items to take count (page size)
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// (Optional) Order by field
        /// </summary>
        public PublicationsOrderByFields? OrderByField { get; set; }

        /// <summary>
        /// (Optional) Order direction
        /// </summary>
        public OrderTypes? OrderType { get; set; }
    }
}
