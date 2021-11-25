using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Contract to get option screener data
    /// </summary>
    public class GetOptionScreenerDataContract
    {
        /// <summary>
        /// Symbol ID.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Trade type.
        /// </summary>
        public TradeTypes TradeType { get; set; }

        /// <summary>
        /// (Optional) Option strategy type.
        /// </summary>
        public InvestmentStrategyTypes? Strategy { get; set; }
    }
}
