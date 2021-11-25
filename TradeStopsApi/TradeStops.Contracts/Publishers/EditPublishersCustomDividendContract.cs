using System;
using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters for editing a custom dividend
    /// </summary>
    public class EditPublishersCustomDividendContract
    {
        /// <summary>
        /// Custom dividend id
        /// </summary>
        public int CustomDividendId { get; set; }

        /// <summary>
        /// (optional) New trade date
        /// </summary>
        public Optional<DateTime> TradeDate { get; set; }

        /// <summary>
        /// (optional) New custom dividend value
        /// </summary>
        public Optional<decimal> DividendAmount { get; set; }

        /// <summary>
        /// (optional) New custom dividend description
        /// </summary>
        public Optional<string> Description { get; set; }
    }
}
