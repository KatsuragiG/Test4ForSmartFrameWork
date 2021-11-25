using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Alerts grid row
    /// </summary>
    public class PtAlertsGridRowContract
    {
        /// <summary>
        /// See Position Trigger contract values.
        /// </summary>
        public PtPositionTriggerContract PositionTrigger { get; set; }

        /// <summary>
        /// Position ID.
        /// </summary>
        public int PositionId { get; set; }

        /// <summary>
        /// Symbol data.
        /// </summary>
        public SymbolContract Symbol { get; set; }

        /// <summary>
        /// Position portfolio ID
        /// </summary>
        public int PortfolioId { get; set; }

        /// <summary>
        /// Position portfolio name.
        /// </summary>
        public string PortfolioName { get; set; }

        /// <summary>
        /// Total position statistics calculated using corresponding (Open, Closed, All) trades.
        /// </summary>
        public PortfolioTrackerPositionStatsContract PositionStats { get; set; }

        /// <summary>
        /// Position type in the TradeStops
        /// </summary>
        public PositionTypes PositionType { get; set; }

        /// <summary>
        /// Position trade type. 
        /// </summary>
        public TradeTypes TradeType { get; set; }

        /// <summary>
        /// Position notes.
        /// </summary>
        public string Notes1 { get; set; }

        /// <summary>
        /// Position close price.
        /// </summary>
        public decimal LatestClose { get; set; }

        /// <summary>
        /// Latest close price of underlying stock for option.
        /// </summary>
        public decimal? ParentLatestClose { get; set; }

        /// <summary>
        /// Latest Intraday price.
        /// </summary>
        public decimal? LatestIntradayPrice { get; set; }

        /// <summary>
        /// Contract containing SSI values.
        /// </summary>
        public SsiValueContract SsiValue { get; set; }

        /// <summary>
        /// Position VQ value.
        /// </summary>
        public decimal? VolatilityQuotient { get; set; }

        /// <summary>
        /// Currency ID of the position.
        /// </summary>
        public int CurrencyId { get; set; }

        /// <summary>
        /// Price when the Position Trigger will be triggered. 
        /// </summary>
        public decimal? TriggerPrice { get; set; }

        /// <summary>
        /// Amount of money to risk. 
        /// </summary>
        public decimal? Risk { get; set; }

        /// <summary>
        /// Defines that Position Tigger has been triggered.
        /// </summary>
        public bool IsTriggered { get; set; }

        /// <summary>
        /// The number of calendar days since the Position Trigger has been triggered.
        /// </summary>
        public int DaysTriggered { get; set; }

        /// <summary>
        /// Earliest date when Position Trigger has been triggered.
        /// </summary>
        public DateTime? FirstTimeTriggered { get; set; }

        /// <summary>
        /// Latest date when Position Trigger has been triggered.
        /// </summary>
        public DateTime? LastTriggered { get; set; }
    }
}
