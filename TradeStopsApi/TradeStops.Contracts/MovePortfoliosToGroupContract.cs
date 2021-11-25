using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to move portfolios into new Portfolio Group.
    /// </summary>
    public class MovePortfoliosToGroupContract
    {
        /// <summary>
        /// ID of the target Portfolio Group.
        /// Can be null - in this case portfolios will be moved to 'No Group' group, so Portfolio.PortfolioGroupId will be null in result.
        /// </summary>
        public int? PortfolioGroupId { get; set; }

        /// <summary>
        /// IDs of the portfolios to move into target Portfolio Group.
        /// Note that all portfolios must belong to the same Organization, as the Portfolio Group.
        /// </summary>
        public List<int> PortfolioIds { get; set; }
    }
}
