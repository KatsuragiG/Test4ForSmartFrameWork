using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Statistics for portfolio rule runs.
    /// </summary>
    public class PortfolioRuleRunResultStatisticsContract
    {
        /// <summary>
        /// User Snaid.
        /// </summary>
        public string Snaid { get; set; }

        /// <summary>
        /// User Id. (Can be null if the user isn't find).
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Failed portfolios.
        /// </summary>
        public int FailedPortfoliosCount { get; set; }

        /// <summary>
        /// Portfolio rule run result.
        /// </summary>
        public PortfolioRuleRunResultTypes PortfolioRuleRunResultType { get; set; }

        /// <summary>
        /// User's email. (Can be null if the user doesn't find).
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User's first name. (Can be null if the user doesn't find).
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// User's last name. (Can be null if the user doesn't find).
        /// </summary>
        public string LastName { get; set; }
    }
}
