using System;
using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Data for calculation of gain.
    /// </summary>
    public class PublishersGainRecalculationContract
    {
        /// <summary>
        /// Open price.
        /// </summary>
        public decimal? OpenPrice { get; set; }

        /// <summary>
        /// Open date.
        /// </summary>
        public DateTime? OpenDate { get; set; }

        /// <summary>
        /// Contains the current price for open positions and close price for close positions.
        /// </summary>
        public decimal CurrentClosePrice { get; set; }

        /// <summary>
        /// Contains the current date for open positions and close date for close positions.
        /// </summary>
        public DateTime CurrentCloseDate { get; set; }

        /// <summary>
        /// Max down side price for regular position.
        /// </summary>
        public decimal? MaxDownSide { get; set; }

        /// <summary>
        /// Trade type.
        /// </summary>
        public PublishersTradeTypes TradeType { get; set; }

        /// <summary>
        /// Defines whether Dividend Reinvestment Plan is set.
        /// </summary>
        public bool IsDrip { get; set; }

        /// <summary>
        /// Defines whether symbol is custom.
        /// </summary>
        public bool IsCustomSymbol { get; set; }

        /// <summary>
        /// Weight on open date.
        /// </summary>
        public decimal OpenWeight { get; set; }

        /// <summary>
        /// Weight on current or close date.
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// Strike price.
        /// </summary>
        public decimal? StrikePrice { get; set; }

        /// <summary>
        /// Margin.
        /// </summary>
        public decimal? Margin { get; set; }

        /// <summary>
        /// Subtrade type.
        /// </summary>
        public PublishersSubtradeTypes SubtradeType { get; set; }

        /// <summary>
        /// Defines whether position is based on a put option.
        /// </summary>
        public bool IsBasedOnPutOption { get; set; }

        /// <summary>
        /// Defines whether position is opened or waiting.
        /// </summary>
        public bool IsOpenedOrWaiting { get; set; }

        /// <summary>
        /// Dividends of HD symbol.
        /// </summary>
        public List<PublishersDividendRecalculationContract> Dividends { get; set; }

        /// <summary>
        /// Dividends of сustom symbol.
        /// </summary>
        public List<PublishersCustomDividendRecalculationContract> CustomDividends { get; set; }
    }
}
