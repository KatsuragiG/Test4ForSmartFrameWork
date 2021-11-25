namespace TradeStops.Common.Enums
{
    /// <summary>
    ///  Search in repository with the next match mode:
    ///    VendorAccountId - Exact match mode
    ///    UserEmail - Start match mode
    ///    VendorUsername - Exact match mode
    ///    FinancialInstitutionName - Anywhere match mode
    ///    ErrorDescription - Anywhere match mode
    ///    LinkSessionId - Exact match mode
    /// </summary>
    public enum SearchSyncReportsAccountsStatisticsSearchByFields
    {
        VendorAccountId = 1,

        UserEmail = 2,

        VendorUsername = 3,

        FinancialInstitutionName = 4,

        ErrorDescription = 5,

        LinkSessionId = 6
    }
}
