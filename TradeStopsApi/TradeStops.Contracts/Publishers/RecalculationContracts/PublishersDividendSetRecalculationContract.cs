using System;
using System.Collections.Generic;
using TradeStops.Contracts.Interfaces;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Dividend set data for calculations.
    /// </summary>
    public class PublishersDividendSetRecalculationContract
    {
        /// <summary>
        /// Active dividends.
        /// </summary>
        public IEnumerable<IPublishersDividend> ActiveDividends { get; set; }

        /// <summary>
        /// Start date for calculations.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Finish date for calculations.
        /// </summary>
        public DateTime FinishDate { get; set; }

        /// <summary>
        /// Defines whether Dividend Reinvestment Plan is set.
        /// </summary>
        public bool IsDrip { get; set; }

        /// <summary>
        /// Weight.
        /// </summary>
        public decimal Weight { get; set; }
    }
}
