using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class GetSystemEventCategoriesStatisticsContract
    {
        /// <summary>
        /// List of PortfolioIds to show statistics for portfolios (including delisted portfolios without HideHistory option)
        /// </summary>
        public List<int> PortfolioIds { get; set; }

        /// <summary>
        /// System event categories to show statistics. Event categories for corporate actions are not valid for this statistics
        /// </summary>
        public List<SystemEventCategories> Categories { get; set; }
    }
}
