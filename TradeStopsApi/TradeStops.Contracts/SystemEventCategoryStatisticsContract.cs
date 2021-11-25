using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class SystemEventCategoryStatisticsContract
    {
        /// <summary>
        /// System events category ID
        /// </summary>
        public SystemEventCategories SystemEventCategoryId { get; set; }

        /// <summary>
        /// Number of events for specified category
        /// </summary>
        public int EventsCount { get; set; }
    }
}
