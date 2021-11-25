using System;
using TradeStops.Common.Enums;
using TradeStops.Contracts.Interfaces;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Dividend data for calculations.
    /// </summary>
    public class PublishersDividendRecalculationContract : IPublishersDividend
    {
        /// <summary>
        /// Split adjusted dividend amount.
        /// </summary>
        public decimal DividendAmount { get; set; }

        /// <summary>
        /// Dividend issue date.
        /// </summary>
        public DateTime TradeDate { get; set; }

        /// <summary>
        /// Dividend pay date.
        /// </summary>
        public DateTime PayDate { get; set; }

        /// <summary>
        /// Defines whether dividend is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Non adjusted dividend amount.
        /// </summary>
        public decimal NonAdjustedValue { get; set; }

        /// <summary>
        /// Volume weighted average price.
        /// </summary>
        public decimal? Vwap { get; set; }

        /// <summary>
        /// Cross rate to the US dollar on the TradeDate.
        /// </summary>
        public decimal? CrossToUsdTradeDate { get; set; }

        /// <summary>
        /// Defines conversion status.
        /// </summary>
        public PublishersConversionStatuses ConversionStatus { get; set; }
    }
}
