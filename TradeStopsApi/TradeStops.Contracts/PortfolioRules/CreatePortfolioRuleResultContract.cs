using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Contract for creating the result of portfolio rule.
    /// </summary>
    public class CreatePortfolioRuleResultContract
    {
        /// <summary>
        /// Portfolio Rule Id for which the result is being generated.
        /// </summary>
        public int PortfolioRuleId { get; set; }

        /// <summary>
        /// User snaid for which the result is being generated
        /// </summary>
        public string Snaid { get; set; }

        /// <summary>
        /// User id for which the result is being generated.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// The number of portfolios that were not created
        /// </summary>
        public int FailedPortfoliosCount { get; set; }

        /// <summary>
        /// Result type.
        /// </summary>
        public PortfolioRuleRunResultTypes PortfolioRuleRunResultType { get; set; }
    }
}
