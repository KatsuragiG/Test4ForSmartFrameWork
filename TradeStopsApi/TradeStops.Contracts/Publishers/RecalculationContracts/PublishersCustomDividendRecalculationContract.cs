using System;
using TradeStops.Contracts.Interfaces;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Custom dividend data for calculations.
    /// </summary>
    public class PublishersCustomDividendRecalculationContract : IPublishersDividend
    {
        /// <summary>
        /// Dividend amount.
        /// </summary>
        public decimal DividendAmount { get; set; }

        /// <summary>
        /// Dividend issue date.
        /// </summary>
        public DateTime TradeDate { get; set; }
    }
}
