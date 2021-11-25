namespace TradeStops.WebApi.Client.Configuration
{
    public class ManualClientCacheSettings : IClientCacheSettings
    {
        public int TempItemsExpirationInSeconds { get; set; }

        public int CurrenciesExpirationInSeconds { get; set; }

        public int CrossCoursesExpirationInSeconds { get; set; }

        public int FinancialInstitutionsExpirationInSeconds { get; set; }

        public int PureQuantIndicatorChartDataExpirationInSeconds { get; set; }

        public int VendorSyncErrorsExpirationInSeconds { get; set; }

        public int UserAdminFeaturesExpirationInSeconds { get; set; }
    }
}
