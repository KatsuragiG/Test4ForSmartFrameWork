using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters for creating a custom dividend
    /// </summary>
    public class CreatePublishersCustomDividendContract
    {
        /// <summary>
        /// Trade date
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
        /// Custom symbol id
        /// </summary>
        public virtual int CustomSymbolId { get; set; }
    }
}
