using System;

namespace TradeStops.Contracts.Interfaces
{
    /// <summary>
    /// Dividend.
    /// </summary>
    public interface IPublishersDividend
    {
        /// <summary>
        /// Dividend amount.
        /// </summary>
        decimal DividendAmount { get; }

        /// <summary>
        /// Trade date.
        /// </summary>
        DateTime TradeDate { get; }
    }
}
