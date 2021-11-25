using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// SSI value
    /// </summary>
    public class SsiValueContract
    {
        /// <summary>
        /// Current SSI state. SSI values available only for stocks.
        /// </summary>
        public SsiStatuses CurrentValue { get; set; }

        /// <summary>
        /// Date when the SSI state was changed.
        /// </summary>
        public DateTime? ChangeDate { get; set; }

        /// <summary>
        /// Price of the stock at which the SSI state will be changed to StoppedOut.
        /// </summary>
        public decimal? StopPrice { get; set; }

        /// <summary>
        /// Date when Entry Signal was triggered.
        /// </summary>
        public DateTime? ReEntryDate { get; set; }

        /// <summary>
        /// Minimal VQ value of the stock.
        /// </summary>
        public decimal? MinVq { get; set; }

        /// <summary>
        /// Highest or lowest price of the stock depends on position trade type (Long or Short).
        /// </summary>
        public decimal? ExtremumPrice { get; set; }

        /// <summary>
        /// Date when the highest or lowest stock price was reached.
        /// </summary>
        public DateTime? ExtremumDate { get; set; }

        /// <summary>
        /// Latest price of the stock.
        /// </summary>
        public decimal? LatestPrice { get; set; }
    }
}
