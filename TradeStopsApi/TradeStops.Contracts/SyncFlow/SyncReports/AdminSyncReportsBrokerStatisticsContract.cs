namespace TradeStops.Contracts
{
    /// <summary>
    /// Statistics data for a single broker for sync reports.
    /// </summary>
    public class AdminSyncReportsBrokerStatisticsContract
    {
        /// <summary>
        ///  Financial institution name.
        /// </summary>
        public string FinancialInstitutionName { get; set; }

        /// <summary>
        ///  Unique vendor accounts count.
        /// </summary>
        public int VendorAccountsCount { get; set; }

        /// <summary>
        ///  Successful sync attempts total count.
        /// </summary>
        public int SuccessfulSyncAttemptsTotalCount { get; set; }

        /// <summary>
        ///  Failed sync attempts total count.
        /// </summary>
        public int FailedSyncAttemptsTotalCount { get; set; }

        /// <summary>
        ///  Rate for failed sync attempts.
        /// </summary>
        public decimal FailedSyncRate { get; set; }

        /// <summary>
        ///  Rate for successfull refresh attempts.
        /// </summary>
        public decimal SuccessfulRefreshRate { get; set; }

        /// <summary>
        /// Financial institution notes
        /// </summary>
        public string FinancialInstitutionNotes { get; set; }
    }
}
