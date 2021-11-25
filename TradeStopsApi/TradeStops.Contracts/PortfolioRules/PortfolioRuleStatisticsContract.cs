namespace TradeStops.Contracts
{
    /// <summary>
    /// Portfolio rule with statistics
    /// </summary>
    public class PortfolioRuleStatisticsContract
    {
        /// <summary>
        /// The number of users processed by the rule.
        /// </summary>
        public int ProcessedUsersCount { get; set; }

        /// <summary>
        /// The number of failed portfolios by the rule.
        /// </summary>
        public int FailedPortfoliosCount { get; set; }

        /// <summary>
        /// Current portfolio rule.
        /// </summary>
        public PortfolioRuleContract PortfolioRule { get; set; }
    }
}
