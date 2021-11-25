using System;
using System.Collections.Generic;
using System.Text;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Contract for getting the runs statistics of the portfolio rule
    /// </summary>
    public class GetPortfolioRuleRunResultStatisticsContract
    {
        /// <summary>
        /// Portfolio rule ID
        /// </summary>
        public int PortfolioRuleId { get; set; }

        /// <summary>
        /// Items to skip count ((page number - 1) * page size).
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// Items to take count (page size).
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// (optional) Value to search for.
        /// </summary>
        public string Search { get; set; }

        /// <summary>
        /// (optional) Field to use for search value.
        /// </summary>
        public GetPortfolioRuleResultStatisticsSearchByFields? SearchByField { get; set; }

        /// <summary>
        /// Order by field.
        /// </summary>
        public GetPortfolioRuleResultStatisticsOrderByFields OrderByField { get; set; }

        /// <summary>
        /// Order direction.
        /// </summary>
        public OrderTypes OrderType { get; set; }
    }
}
