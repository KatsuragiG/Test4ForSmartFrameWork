using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Group of portfolios for PortfolioTracker Portfolios grid.
    /// Each PortfolioTracker portfolio can belong to single group.
    /// Portfolios that don't belong to any group, belong to virtual (we don't store it in the database) 'No Group' group.
    /// Group can be empty (without any portfolios assigned).
    /// Each group belongs to an organization (not to a user).
    /// </summary>
    public class PortfolioTrackerPortfolioGroupContract
    {
        /// <summary>
        /// ID of the group.
        /// </summary>
        public int PortfolioGroupId { get; set; }

        /// <summary>
        /// ID of the organization that owns the group.
        /// </summary>
        public PortfolioTrackerOrganizations OrganizationId { get; set; }

        /// <summary>
        /// The name of the group.
        /// </summary>
        public string Name { get; set; }
    }
}
