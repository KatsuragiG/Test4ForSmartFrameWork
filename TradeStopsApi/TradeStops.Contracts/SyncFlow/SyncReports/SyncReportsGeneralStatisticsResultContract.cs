namespace TradeStops.Contracts
{
    /// <summary>
    /// General statistics data for sync reports.
    /// </summary>
    public class SyncReportsGeneralStatisticsResultContract
    {
        /// <summary>
        ///  Total successful imports count.
        /// </summary>
        public int SuccessfulImportsTotalCount { get; set; }

        /// <summary>
        ///  Total failed imports count.
        /// </summary>
        public int FailedImportsTotalCount { get; set; }

        /// <summary>
        ///  Total successful resfreshes count.
        /// </summary>
        public int SuccessfulRefreshesTotalCount { get; set; }

        /// <summary>
        ///  Total failed resfreshes count.
        /// </summary>
        public int FailedRefreshesTotalCount { get; set; }
    }
}
