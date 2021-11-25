using System;
using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Investment strategy result
    /// </summary>
    public class StrategyCurrentResultContract
    {
        /// <summary>
        /// Strategy Id
        /// </summary>
        public InvestmentStrategyTypes StrategyId { get; set; }

        /// <summary>
        /// Strategy Name
        /// </summary>
        public string StrategyName { get; set; }

        /// <summary>
        /// The date when this Symbol appeared in this strategy
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Symbol contract
        /// </summary>
        public SymbolContract Symbol { get; set; }

        /// <summary>
        /// Currency contract
        /// </summary>
        public CurrencyContract Currency { get; set; }

        /// <summary>
        /// Latest price contract
        /// </summary>
        public PriceContract LatestPrice { get; set; }

        /// <summary>
        /// Previous price contract
        /// </summary>
        public PriceContract PreviousPrice { get; set; }

        /// <summary>
        /// Current SSI value contract
        /// </summary>
        public SsiCurrentValueContract SsiCurrentValue { get; set; }

        /// <summary>
        /// List of recommended options for stock symbol.
        /// </summary>
        public List<RecommendedOptionContract> RecommendedOptions { get; set; }

        /// <summary>
        /// VQ value
        /// </summary>
        public decimal VqValue { get; set; }        

        /// <summary>
        /// The name of the sector
        /// </summary>
        public string Sector { get; set; }

        /// <summary>
        /// Smith Rank
        /// </summary>
        public decimal SmithRank { get; set; }
    }
}
