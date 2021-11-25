using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create Portfolio Group for PortfolioTracker Portfolios Grid.
    /// </summary>
    public class CreatePortfolioTrackerPortfolioGroupContract
    {
        /// <summary>
        /// The name of the group.
        /// </summary>
        public string Name { get; set; }
    }
}
