using System;
using System.Collections.Generic;
using System.Text;

namespace TradeStops.Contracts
{
    /// <summary>
    /// The contract contains the result of the value allocation grouped by symbols.
    /// </summary>
    public class ValueAllocationContract
    {
        /// <summary>
        /// The allocation groups by the symbol
        /// </summary>
        public List<ValueAllocationSymbolGroupContract> Groups { get; set; }

        /// <summary>
        /// The currency of positions value
        /// </summary>
        public int CurrencyId { get; set; }

        /// <summary>
        /// The total value of all positions.
        /// </summary>
        public decimal TotalValue { get; set; }
    }
}
