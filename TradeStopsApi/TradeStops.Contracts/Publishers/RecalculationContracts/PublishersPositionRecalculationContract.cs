using System;
using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Position data for calculations. 
    /// </summary>
    public class PublishersPositionRecalculationContract
    {
        /// <summary>
        /// Position Id.
        /// </summary>
        public int PositionId { get; set; }

        /// <summary>
        /// Position type.
        /// </summary>
        public PublishersPositionTypes PositionType { get; set; }

        /// <summary>
        /// Trade status.
        /// </summary>
        public PublishersTradeStatuses? TradeStatus { get; set; }

        /// <summary>
        /// Open price.
        /// </summary>
        public decimal? OpenPrice { get; set; }

        /// <summary>
        /// Open date.
        /// </summary>
        public DateTime? OpenDate { get; set; }

        /// <summary>
        /// Close price.
        /// </summary>
        public decimal? ClosePrice { get; set; }

        /// <summary>
        /// Current price.
        /// </summary>
        public decimal? CurrentPrice { get; set; }

        /// <summary>
        /// Margin.
        /// </summary>
        public decimal? Margin { get; set; }

        /// <summary>
        /// Total credit.
        /// </summary>
        public decimal? TotalCredit { get; set; }

        /// <summary>
        /// High close price.
        /// </summary>
        public decimal? HighClosePrice { get; set; }

        /// <summary>
        /// Position holding period.
        /// </summary>
        public int HoldPeriod { get; set; }

        /// <summary>
        /// Defines whether position is opened.
        /// </summary>
        public bool IsOpened { get; set; }

        /// <summary>
        /// Weight on open date.
        /// </summary>
        public decimal OpenWeight { get; set; }

        /// <summary>
        /// Weight on current or close date.
        /// </summary>
        public decimal? Weight { get; set; }

        /// <summary>
        /// Purchase obligation.
        /// </summary>
        public decimal? PurchaseObligation { get; set; }

        /// <summary>
        /// Sum of dividends.
        /// </summary>
        public decimal? Dividends { get; set; }

        /// <summary>
        /// Percent gain.
        /// </summary>
        public decimal? Gain { get; set; }

        /// <summary>
        /// Dollar gain.
        /// </summary>
        public decimal DollarGain { get; set; }

        /// <summary>
        /// Capital gains yield.
        /// </summary>
        public decimal? CapitalGainsYield { get; set; }

        /// <summary>
        /// Cross rate to the US dollar for gain calculation.
        /// </summary>
        public decimal? GainCurrency { get; set; }

        /// <summary>
        /// Defines whether position is published.
        /// </summary>
        public bool IsPublished { get; set; }

        /// <summary>
        /// Currency type.
        /// </summary>
        public PublishersCurrencyTypes? CurrencyType { get; set; }

        /// <summary>
        /// Defines conversion status.
        /// </summary>
        public PublishersConversionStatuses ConversionStatus { get; set; }

        /// <summary>
        /// Position settings.
        /// </summary>
        public PublishersPositionSettingRecalculationContract PositionSetting { get; set; }

        /// <summary>
        /// Trades that make up the position.
        /// </summary>
        public List<PublishersSubtradeRecalculationContract> Subtrades { get; set; }
    }
}
