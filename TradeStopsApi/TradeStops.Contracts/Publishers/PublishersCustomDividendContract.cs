using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Custom dividend contract
    /// </summary>
    public class PublishersCustomDividendContract
    {
        /// <summary>
        /// Custom dividend id
        /// </summary>
        public int CustomDividendId { get; set; }

        /// <summary>
        /// Trade date of custom dividend
        /// </summary>
        public DateTime TradeDate { get; set; }

        /// <summary>
        /// Custom dividend value
        /// </summary>
        public decimal DividendAmount { get; set; }

        /// <summary>
        /// Custom dividend description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Custom symbol id for dividend
        /// </summary>
        public int CustomSymbolId { get; set; }
    }
}
