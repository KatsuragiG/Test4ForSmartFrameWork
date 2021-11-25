using System;
using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to search publications
    /// </summary>
    public class SearchPublicationsContract
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
        /// Items to skip count
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// Items to take count (page size).
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Order by field.
        /// </summary>
        public PublicationsOrderByFields OrderByField { get; set; }

        /// <summary>
        /// Order direction.
        /// </summary>
        public OrderTypes OrderType { get; set; }
    }
}
