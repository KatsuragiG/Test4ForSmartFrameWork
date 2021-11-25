using System;
using System.Collections.Generic;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class GetClosedPositionsGridDataContract
    {
        /// <summary>
        /// Array of portfolio IDs.
        /// </summary>
        public List<int> PortfolioIds { get; set; }

        /// <summary>
        /// Defines the beginning of the position close dates range.
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Defines the ending of the position close dates range.
        /// </summary>
        public DateTime ToDate { get; set; }
    }
}
