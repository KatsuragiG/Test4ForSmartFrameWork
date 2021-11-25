namespace TradeStops.Contracts
{
    /// <summary>
    /// Information about user's active portfolios, alerts, newsletters
    /// </summary>
    public class ZendeskAppsPortfoliosInfoContract
    {
        /// <summary>
        /// Number of active portfolios that user has in the account
        /// </summary>
        public int PortfoliosCount { get; set; }

        /// <summary>
        /// Max allowed number of portfolios that user can create
        /// </summary>
        public int MaxPortfoliosCount { get; set; }

        /// <summary>
        /// Number of active alerts that user has in the account
        /// </summary>
        public int AlertsCount { get; set; }

        /// <summary>
        /// Max allowed number of alerts that user can create
        /// </summary>
        public int MaxAlertsCount { get; set; }

        /// <summary>
        /// Number of newsletters that available to user by subscription
        /// </summary>
        public int NewslettersCount { get; set; }
    }
}
