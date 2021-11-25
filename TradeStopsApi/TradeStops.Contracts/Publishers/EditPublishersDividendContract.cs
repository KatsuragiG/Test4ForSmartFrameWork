using System;
using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters for editing dividend
    /// </summary>
    public class EditPublishersDividendContract
    {
        /// <summary>
        /// Dividend Id.
        /// </summary>
        public int DividendId { get; set; }

        /// <summary>
        /// (optional) New date when the dividend is announced.
        /// </summary>
        public Optional<DateTime> TradeDate { get; set; }

        /// <summary>
        /// (optional) New date when the dividend is payed.
        /// </summary>
        public Optional<DateTime> PayDate { get; set; }

        /// <summary>
        /// (optional) New non adjusted value.
        /// </summary>
        public Optional<decimal> NonAdjustedValue { get; set; }

        /// <summary>
        /// (optional) Description.
        /// </summary>
        public Optional<string> Description { get; set; }

        /// <summary>
        /// (optional) Indicates whether the dividend is included into related calculations or not.
        /// </summary>
        public Optional<bool> IsActive { get; set; }
    }
}
