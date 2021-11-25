using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Intraday Trade Close contract
    /// </summary>
    public class IntradayTradeCloseContract
    {
        /// <summary>
        /// Symbol ID.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Trade date.
        /// </summary>
        public DateTime TradeDate { get; set; }

        /// <summary>
        /// Closing price for the given Trade Date. This value is not adjusted by historical corporate actions.
        /// </summary>
        public decimal TradeClose { get; set; }
    }
}
