#if NET47
using System.Configuration;

namespace TradeStops.WebApi.Client.Configuration
{
    public class ClientCacheSettings : ConfigurationSection, IClientCacheSettings
    {
        [ConfigurationProperty(nameof(TempItemsExpirationInSeconds), IsRequired = true)]
        public int TempItemsExpirationInSeconds => (int)base[nameof(TempItemsExpirationInSeconds)];

        [ConfigurationProperty(nameof(CurrenciesExpirationInSeconds), IsRequired = false)]
        public int CurrenciesExpirationInSeconds => (int)base[nameof(CurrenciesExpirationInSeconds)];

        [ConfigurationProperty(nameof(CrossCoursesExpirationInSeconds), IsRequired = false)]
        public int CrossCoursesExpirationInSeconds => (int)base[nameof(CrossCoursesExpirationInSeconds)];

        [ConfigurationProperty(nameof(FinancialInstitutionsExpirationInSeconds), IsRequired = false)]
        public int FinancialInstitutionsExpirationInSeconds => (int)base[nameof(FinancialInstitutionsExpirationInSeconds)];

        [ConfigurationProperty(nameof(PureQuantIndicatorChartDataExpirationInSeconds), IsRequired = false)]
        public int PureQuantIndicatorChartDataExpirationInSeconds => (int)base[nameof(PureQuantIndicatorChartDataExpirationInSeconds)];

        [ConfigurationProperty(nameof(VendorSyncErrorsExpirationInSeconds), IsRequired = false)]
        public int VendorSyncErrorsExpirationInSeconds => (int)base[nameof(VendorSyncErrorsExpirationInSeconds)];

        [ConfigurationProperty(nameof(UserAdminFeaturesExpirationInSeconds), IsRequired = false)]
        public int UserAdminFeaturesExpirationInSeconds => (int)base[nameof(UserAdminFeaturesExpirationInSeconds)];
    }
}
#endif