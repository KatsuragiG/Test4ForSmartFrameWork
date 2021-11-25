using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Information about non-working and reduced trading days on exchanges
    /// </summary>
    public class ExchangeHolidayContract
    {
        /// <summary>
        /// ID of the exchange
        /// </summary>
        public virtual int ExchangeId { get; set; }

        /// <summary>
        /// Trade date
        /// </summary>
        public virtual DateTime TradeDate { get; set; }

        /// <summary>
        /// Description of the holiday. Can be null.
        /// </summary>
        public virtual string Name { get; set; }
    }
}
