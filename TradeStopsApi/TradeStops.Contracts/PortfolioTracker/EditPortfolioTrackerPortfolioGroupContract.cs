using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to edit Portfolio Group.
    /// </summary>
    public class EditPortfolioTrackerPortfolioGroupContract
    {
        /// <summary>
        /// (Optional) The name of the group.
        /// </summary>
        public Optional<string> Name { get; set; }
    }
}
