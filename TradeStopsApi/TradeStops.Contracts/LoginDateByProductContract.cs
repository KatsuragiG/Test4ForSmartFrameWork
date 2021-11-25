using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// The information about last successful login attempt into product
    /// </summary>
    public class LoginDateByProductContract
    {
        /// <summary>
        /// ID of the Product
        /// </summary>
        public Products ProductId { get; set; }

        /// <summary>
        /// Last successful login date (EST timezone)
        /// </summary>
        public DateTime LoginDate { get; set; }
    }
}
