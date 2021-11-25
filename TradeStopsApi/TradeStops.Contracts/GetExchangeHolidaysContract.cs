using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to get Exchange Holidays
    /// </summary>
    public class GetExchangeHolidaysContract
    {
        /// <summary>
        /// The first date of the range
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// The last date of the range
        /// </summary>
        public DateTime ToDate { get; set; }

        /// <summary>
        /// Id of the Exchange Country
        /// </summary>
        public ExchangeCountryTypes ExchangeCountryId { get; set; }
    }
}
