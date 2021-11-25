using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Contract for creating portfolio creation rules.
    /// </summary>
    public class CreatePortfolioRuleContract
    {
        /// <summary>
        /// Campaign Id. Should be specified only for automatic rules.
        /// </summary>
        public string CampaignId { get; set; }

        /// <summary>
        /// Campaign name. Should be specified only for automatic rules.
        /// </summary>
        public string CampaignName { get; set; }

        /// <summary>
        /// Portfolio IDs from which new portfolios are created for users.
        /// </summary>
        public List<int> SourcePortfolioIds { get; set; }

        /// <summary>
        /// User from whom source portfolios were taken
        /// </summary>
        public int SourceUserId { get; set; }

        /// <summary>
        /// Snaids of users to whom new portfolios will be created. Should be specified only for manual rules.
        /// </summary>
        public List<string> UserSnaids { get; set; }

        /// <summary>
        /// Rule type.
        /// </summary>
        public PortfolioRuleTypes PortfolioRuleType { get; set; }

        /// <summary>
        /// Сurrent status of the rule.
        /// </summary>
        public PortfolioRuleStatusTypes PortfolioRuleStatusType { get; set; }
    }
}
