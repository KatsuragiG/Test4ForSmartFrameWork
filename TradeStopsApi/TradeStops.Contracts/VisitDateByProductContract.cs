using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// The information about last visit into product
    /// </summary>
    public class VisitDateByProductContract
    {
        /// <summary>
        /// ID of the Product
        /// </summary>
        public Products ProductId { get; set; }

        /// <summary>
        /// Last visit date (EST timezone)
        /// </summary>
        public DateTime VisitDate { get; set; }
    }
}
