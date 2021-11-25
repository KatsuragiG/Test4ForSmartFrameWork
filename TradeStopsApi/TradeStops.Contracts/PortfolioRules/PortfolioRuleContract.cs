using System;
using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Portfolio Rule
    /// </summary>
    public class PortfolioRuleContract
    {
        /// <summary>
        /// Unique portfolio rule Id.
        /// </summary>
        public int PortfolioRuleId { get; set; }

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
        /// Snaids of users to whom new portfolios are created. Should be specified only for manual rules.
        /// </summary>
        public List<string> UserSnaids { get; set; }

        /// <summary>
        /// Rule type.
        /// </summary>
        public PortfolioRuleTypes PortfolioRuleType { get; set; }

        /// <summary>
        /// Сurrent status of rule.
        /// </summary>
        public PortfolioRuleStatusTypes PortfolioRuleStatusType { get; set; }

        /// <summary>
        /// Task creation date (UTC)
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Task updating date (UTC)
        /// </summary>
        public DateTime UpdateDate { get; set; }
    }
}
