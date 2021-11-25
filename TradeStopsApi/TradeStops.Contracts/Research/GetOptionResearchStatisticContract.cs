using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to get option research statistic.
    /// </summary>
    public class GetOptionResearchStatisticContract
    {
        /// <summary>
        /// Symbol ID.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Trade type.
        /// </summary>
        public TradeTypes TradeType { get; set; }
    }
}
