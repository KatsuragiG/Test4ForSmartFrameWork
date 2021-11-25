using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// The contract returns the working hours of the exchange.
    /// </summary>
    public class ExchangeDataContract
    {
        /// <summary>
        /// Exchange Id.
        /// </summary>
        public int ExchangeId { get; set; }

        /// <summary>
        /// Exchange day. (UTC)
        /// </summary>
        public DateTime TradeDate { get; set; }

        /// <summary>
        /// Exchange opening time. May be null if the exchange does not work. (UTC)
        /// </summary>
        public DateTime? StartTradeDate { get; set; }

        /// <summary>
        /// Exchange closing time. May be null if the exchange does not work. (UTC)
        /// </summary>
        public DateTime? EndTradeDate { get; set; }

        /// <summary>
        /// Exchange works today or not.
        /// </summary>
        public bool IsWorkingDay { get; set; }

        /// <summary>
        /// Friendly name of exchange.
        /// </summary>
        public string FriendlyName { get; set; }

        /// <summary>
        /// Short name of exchange.
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Nearest previous start time of the exchange. May be null. (UTC)
        /// </summary>
        public DateTime? PreviousStartTradeDate { get; set; }

        /// <summary>
        /// Nearest previous closing time of the exchange. May be null. (UTC)
        /// </summary>
        public DateTime? PreviousEndTradeDate { get; set; }
    }
}
