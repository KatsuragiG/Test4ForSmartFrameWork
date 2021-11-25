using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Option recommended in investment strategy
    /// </summary>
    public class StrategyOptionResultContract
    {
        /// <summary>
        /// Symbol contract for corresponding option
        /// </summary>
        public SymbolContract Symbol { get; set; }

        /// <summary>
        /// Currency contract for symbol
        /// </summary>
        public CurrencyContract Currency { get; set; }

        /// <summary>
        /// Contract with latest price
        /// </summary>
        public PriceContract LatestPrice { get; set; }

        /// <summary>
        /// Price on the date when option has matched strategy criteria
        /// </summary>
        public PriceContract TriggerPrice { get; set; }

        /// <summary>
        /// List of strategies that contain current symbol
        /// </summary>
        public List<InvestmentStrategyContract> Strategies { get; set; }
    }
}
