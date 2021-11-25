using System;
using System.Collections.Generic;
using System.Text;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to get historical Market Outlook state
    /// </summary>
    public class GetHistoricalMarketOutlooksContract
    {
        /// <summary>
        /// Get historical Market Outlook state as of TradeDate
        /// </summary>
        public DateTime TradeDate { get; set; }

        /// <summary>
        /// (Optional) Get historical Market Outlook for a given group
        /// </summary>
        public MarketOutlookGroupIds? GroupId { get; set; }

        /// <summary>
        /// (Optional) Get historical Market Outlook for a given list
        /// </summary>
        public List<MarketOutlookTypes> MarketOutlookTypes { get; set; }
    }
}
