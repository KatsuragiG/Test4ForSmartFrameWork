using System;
using System.Collections.Generic;

using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class GetRecentEventsContract
    {
        /// <summary>
        /// (optional) Categories to load. Leave null or empty to load events for all categories
        /// </summary>
        public List<SystemEventCategories> Categories { get; set; }

        /// <summary>
        /// (optional) List of portfolio IDs to filter events by portfolio. Leave null or empty to load events for all portfolios
        /// </summary>
        public List<int> PortfolioIds { get; set; }

        /// <summary>
        /// (optional) List of Position IDs to filter events by position. Leave null or empty to load events for all positions.
        /// Note, if this list contains any IDs then PortfolioIds list must be empty or contain corresponding PortfolioIds
        /// </summary>
        public List<int> PositionIds { get; set; }

        /// <summary>
        /// (optional) Start date for events to load
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// (optional) End date for events to load
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// (optional) Number of events to load. Set to null to load all events
        /// </summary>
        public int? NumberOfEvents { get; set; }
    }
}
