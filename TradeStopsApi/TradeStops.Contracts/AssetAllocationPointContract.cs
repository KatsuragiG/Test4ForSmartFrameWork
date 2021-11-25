using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Asset allocation point
    /// </summary>
    public class AssetAllocationPointContract
    {
        /// <summary>
        /// Industry or sector group name.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Symbols values.
        /// </summary>
        public List<AssetAllocationSymbolWeightContract> Symbols { get; set; }

        /// <summary>
        /// Weight of category - value in [0..1] interval
        /// </summary>
        public decimal CategoryWeight { get; set; }
    }
}
