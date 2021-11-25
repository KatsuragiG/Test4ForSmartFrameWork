using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to get fun facts
    /// </summary>
    public class GetFunFactsContract
    {
        /// <summary>
        /// Symbol ID.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Fun fact types.
        /// </summary>
        public List<FunFactTypes> FunFactTypes { get; set; }
    }
}
