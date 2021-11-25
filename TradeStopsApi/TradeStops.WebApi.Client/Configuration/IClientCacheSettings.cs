namespace TradeStops.WebApi.Client.Configuration
{
    public interface IClientCacheSettings
    {
        int TempItemsExpirationInSeconds { get; }

        int CurrenciesExpirationInSeconds { get; }

        int CrossCoursesExpirationInSeconds { get; }

        int FinancialInstitutionsExpirationInSeconds { get; }

        int PureQuantIndicatorChartDataExpirationInSeconds { get; }

        int VendorSyncErrorsExpirationInSeconds { get; }

        int UserAdminFeaturesExpirationInSeconds { get; }
    }
}
