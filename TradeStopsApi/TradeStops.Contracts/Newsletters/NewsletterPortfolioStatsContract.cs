using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Newsletter Portfolio statistic values
    /// </summary>
    public class NewsletterPortfolioStatsContract
    {
        /// <summary>
        /// Portfolio ID.
        /// </summary>
        public int PortfolioId { get; set; }

        /// <summary>
        /// Publisher Type Id
        /// </summary>
        public PublisherTypes PublisherType { get; set; }

        /// <summary>
        /// PortfolioStatsTypes of the portfolio stat.
        /// </summary>
        public PortfolioStatsTypes PortfolioStatsType { get; set; }       

        /// <summary>
        /// ID of the portolio currency.
        /// </summary>
        public int CurrencyId { get; set; }

        /// <summary>
        /// Sum of the position gains.
        /// </summary>
        public decimal TotalGain { get; set; }

        /// <summary>
        /// Sum of the daily dollar gains.
        /// </summary>
        public decimal GainDailyTotal { get; set; }

        /// <summary>
        /// Sum of the position cost bases.
        /// </summary>
        public decimal CostBasis { get; set; }        

        /// <summary>
        /// Sum of the position values for the previous trading day.
        /// </summary>
        public decimal PreviousValue { get; set; } 
    }
}
