namespace TradeStops.Contracts
{
    /// <summary>
    /// Available features in the admin area for the user or for subscription
    /// </summary>
    public class AdminFeaturesContract
    {
        /// <summary>
        /// Access to "Users" page and menu visibility, ability to create/edit and disable/enable Users.
        /// </summary>
        public bool UserAccountsPage { get; set; }

        /// <summary>
        /// Access to "Reports" page and menu visibility.
        /// </summary>
        public bool ReportsPage { get; set; }

        /// <summary>
        /// Access to Portfolios and Positions of Users even without permission.
        /// </summary>
        public bool PortfoliosFull { get; set; }

        /// <summary>
        /// Access to "Portfolios" with data of User if User provided access.
        /// </summary>
        public bool Portfolios { get; set; }

        /// <summary>
        /// Access to "Newsletters" with data of User.
        /// </summary>
        public bool Newsletters { get; set; }

        /// <summary>
        /// Access to "Sync Reports" with data of User if User provided access.
        /// </summary>
        public bool SyncReportsPage { get; set; }

        /// <summary>
        /// Access to User Data only if User provided access (1 day, 3 days, 1 month, 1 year, never expire).
        /// </summary>
        public bool UserDataWithUserPermission { get; set; }

        /// <summary>
        /// Full access to User Data even without users agreement.
        /// </summary>
        public bool UserDataWithoutUserPermission { get; set; }

        /// <summary>
        /// Visibility of the subscription for Admin Area product on User page.
        /// </summary>
        public bool AdminAreaSubscriptionVisibility { get; set; }

        /// <summary>
        /// Add/Edit/Delete User with subscription to Admin Area product.
        /// </summary>
        public bool UpdateAgent { get; set; }

        /// <summary>
        /// Access to News Management and visibility of News tab in Admin Area site menu.
        /// </summary>
        public bool News { get; set; }

        /// <summary>
        /// Access to Publication page in Admin Area with rights to create, edit, delete all articles for all publications types.
        /// </summary>
        public bool Publications { get; set; }

        /// <summary>
        /// Access to Find in CRM page.
        /// </summary>
        public bool FindInCrm { get; set; }

        /// <summary>
        /// Permission to completely delete users from database.
        /// </summary>
        public bool DeleteUsers { get; set; }

        /// <summary>
        /// Access to Duplicate Accounts page and permission to merge user accounts.
        /// </summary>
        public bool MergeUsers { get; set; }

        /// <summary>
        /// Access to Find Subscriptions for Newsletters page.
        /// </summary>
        public bool FindNewslettersSubscriptions { get; set; }

        /// <summary>
        /// Access to Financial Institutions Setup page.
        /// </summary>
        public bool FinancialInstitutionsSetup { get; set; }

        /// <summary>
        /// Access to Financial Institutions Rules page.
        /// </summary>
        public bool FinancialInstitutionsRules { get; set; }

        /// <summary>
        /// Access to System Usage page with different metrics.
        /// These metrics display usage of website by users.
        /// </summary>
        public bool SystemUsage { get; set; }

        /// <summary>
        /// Access to White Label page
        /// </summary>
        public bool WhiteLabelPage { get; set; }

        /// <summary>
        /// Access to Data Dog page
        /// </summary>
        public bool DataDogPage { get; set; }

        /// <summary>
        /// Access to report management with draft information.
        /// </summary>
        public bool DraftManagement { get; set; }
    }
}
