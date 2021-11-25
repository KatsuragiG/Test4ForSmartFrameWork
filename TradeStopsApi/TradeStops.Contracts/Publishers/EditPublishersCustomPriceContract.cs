using System;
using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters of editing a custom price
    /// </summary>
    public class EditPublishersCustomPriceContract
    {
        /// <summary>
        /// Custom price id
        /// </summary>
        public int CustomPriceId { get; set; }

        /// <summary>
        /// (optional) New trade date
        /// </summary>
        public Optional<DateTime> Date { get; set; }

        /// <summary>
        /// (optional) New close price
        /// </summary>
        public Optional<decimal> ClosePrice { get; set; }
    }
}
