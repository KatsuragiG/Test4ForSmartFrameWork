using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Portfolio statistic values
    /// </summary>
    public class PortfolioStatsContract
    {
        /// <summary>
        /// Portfolio stat entity ID.
        /// </summary>
        public int PortfolioStatId { get; set; }

        /// <summary>
        /// Portfolio ID.
        /// </summary>
        public int PortfolioId { get; set; }

        /// <summary>
        /// PortfolioStatsTypes of the portfolio stat.
        /// </summary>
        public PortfolioStatsTypes PortfolioStatsType { get; set; }

        /// <summary>
        /// Number of the positions for the corresponding PortfolioStatsTypes.
        /// </summary>
        public int PositionsQuantity { get; set; }

        /// <summary>
        /// Number of the unconfirmed positions.
        /// </summary>
        public int UnconfirmedPositionsQuantity { get; set; }

        /// <summary>
        /// Number of the alerts assigned to the positions.
        /// </summary>
        public int AlertsQuantity { get; set; }

        /// <summary>
        /// Number of the alerts triggered in the past.
        /// </summary>
        public int TriggeredAlertsQuantity { get; set; }

        /// <summary>
        /// Sum of the days held since the acquisition of all portfolio positions.
        /// </summary>
        public int Hold { get; set; }

        /// <summary>
        /// Internal TradeStops value.
        /// </summary>
        public decimal Cash { get; set; }

        /// <summary>
        /// ID of the portolio currency.
        /// </summary>
        public int CurrencyId { get; set; }

        /// <summary>
        /// Sum of the position gains.
        /// </summary>
        public decimal TotalGain { get; set; }

        /// <summary>
        /// Sum of the position cost bases.
        /// </summary>
        public decimal CostBasis { get; set; }

        /// <summary>
        /// Sum of the position values.
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// Sum of the position values for the previous trading day.
        /// </summary>
        public decimal PreviousValue { get; set; }

        /// <summary>
        /// Total of the position dividends.
        /// </summary>
        public decimal DividendsTotal { get; set; }

        /// <summary>
        /// Total Gain E/Dividends
        /// </summary>
        public decimal TotalGainWithoutDividends { get; set; }

        /// <summary>
        /// Sum of the daily dollar gains.
        /// </summary>
        public decimal GainDailyTotal { get; set; }

        /// <summary>
        /// Sum of the position absolute values.
        /// </summary>
        public decimal AbsoluteValue { get; set; }

        /// <summary>
        /// Average portfolio total gain percent.
        /// </summary>
        public decimal AvgTotalGainPercent { get; set; }

        /// <summary>
        /// Average portfolio daily gain percent.
        /// </summary>
        public decimal AvgDailyGainPercent { get; set; }

        /// <summary>
        /// Average hold days of positions.
        /// </summary>
        public int AvgHold { get; set; }
    }
}
