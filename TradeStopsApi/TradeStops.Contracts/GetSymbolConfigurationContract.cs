using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// The contract for getting the symbol configuration.
    /// </summary>
    public class GetSymbolConfigurationContract
    {
        /// <summary>
        /// Symbol Id.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Configuration type.
        /// </summary>
        public List<SymbolConfigurationDataTypes> DataTypes { get; set; }
    }
}
