using System.Collections.Generic;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class GetAlertsGridDataContract
    {
        /// <summary>
        /// Array of portfolio IDs.
        /// </summary>
        public List<int> PortfolioIds { get; set; }

        /// <summary>
        /// Defines that method will return only triggered Position Triggers.
        /// </summary>
        public bool IsTriggeredOnly { get; set; }
    }
}
