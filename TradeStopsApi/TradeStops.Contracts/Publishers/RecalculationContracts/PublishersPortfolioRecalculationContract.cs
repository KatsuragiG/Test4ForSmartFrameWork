using System;
using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Portfolio data for calculations. 
    /// </summary>
    public class PublishersPortfolioRecalculationContract
    {
        /// <summary>
        /// Initial cash.
        /// </summary>
        public decimal InitialCash { get; set; }

        /// <summary>
        /// Last update date.
        /// </summary>
        public DateTime LastUpdateDate { get; set; }

        /// <summary>
        /// Defines conversion status.
        /// </summary>
        public PublishersConversionStatuses ConversionStatus { get; set; }

        /// <summary>
        /// Positions that make up the portfolio.
        /// </summary>
        public List<PublishersPositionRecalculationContract> Positions { get; set; }
    }
}
