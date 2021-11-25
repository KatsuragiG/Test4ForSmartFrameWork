using System;
using System.Collections.Generic;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class GetCombinedVqContract
    {
        /// <summary>
        /// All calculations will be performed in this currency in case positions were provided in multiple currencies.
        /// </summary>
        public int DefaultCurrencyId { get; set; }

        /// <summary>
        /// Trade Date (example: 2018-03-05).
        /// </summary>
        public DateTime TradeDate { get; set; }

        /// <summary>
        /// An array of position values.
        /// </summary>
        public List<GetCombinedVqPositionContract> Positions { get; set; }
    }
}
