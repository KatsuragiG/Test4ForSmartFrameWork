using System.Collections.Generic;
using TradeStops.Common.Enums;
using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Сontract for portfolio creation editing rules. Only automatic rules can be edited.
    /// </summary>
    public class EditPortfolioRuleContract
    {
        /// <summary>
        /// Unique portfolio rule Id.
        /// </summary>
        public int PortfolioRuleId { get; set; }

        /// <summary>
        /// (Optional) Campaign Id. Can be specified only for automatic rules.
        /// </summary>
        public Optional<string> CampaignId { get; set; }

        /// <summary>
        /// (Optional) Campaign Name. Can be specified only for automatic rules.
        /// </summary>
        public Optional<string> CampaignName { get; set; }

        /// <summary>
        /// (Optional) Portfolio IDs from which new portfolios are created for users.
        /// </summary>
        public Optional<List<int>> SourcePortfolioIds { get; set; }

        /// <summary>
        /// (Optional) User from whom source portfolios were taken. Must be specified if portfolioIds change.
        /// </summary>
        public Optional<int> SourceUserId { get; set; }

        /// <summary>
        /// (Optional Сurrent status of the rule.
        /// </summary>
        public Optional<PortfolioRuleStatusTypes> PortfolioRuleStatusType { get; set; }
    }
}
