using System;
using System.Collections.Generic;
using System.Text;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters for getting most popular tickers
    /// </summary>
    public class GetMostPopularTickersContract
    {
        /// <summary>
        /// Maximum number of tickers
        /// </summary>
        public int MaximumNumberOfTickers { get; set; }

        /// <summary>
        /// Tickers asset types
        /// </summary>
        public List<AssetTypes> AssetTypes { get; set; }
    }
}
