using TradeStops.Common.Enums;

namespace TradeStops.Common.Constants
{
    public static class DefaultUserSettings
    {
        public const string DefaultAlertsLanguage = "en-US";
        public const int DefaultMaxConsecutiveAlerts = 1; // also there are a default value in options.js -> form.findField('maxConsecutiveAlerts').setValue(1);
        public const int DefaultDaysBeforeAlertDelete = 100;
        public const bool DefaultIgnoreDividend = false;
        public const bool DefaultSendPreCloseAlerts = true;
        public const int DefaultMaxDaysArchiveCancelAlerts = 30;
        public const int DefaultPageSize = 25;
        public const string DefaultEntryPage = "CbeHome";
        public const decimal DefaultEntryCommission = 0;
        public const decimal DefaultExitCommission = 0;
        public const string DefaultTabOnPositionAlertsPage = "positionsTab";
        public const bool DefaultNewHighProfitAlerts = false;
        public const bool DefaultRemoveAlertsOnceTriggered = false;
        public const bool DefaultSendAdHocEmail = true;
        public const bool DefaultEnableImportPortfolios = false;
        public const int DefaultAlertStartTime = 0;
        public const int DefaultAlertEndTime = 24;
        public const int DefaultTimeZone = -6;
        public const string DefaultTimeZoneId = "Eastern Standard Time";
        public const bool DefaultSendCorporateActionNotificationsForInvestmentPortfolios = true;
        public const bool DefaultSendCorporateActionNotificationsForWatchPortfolios = true;
        public const bool DefaultSendVqNotificationsForInvestmentPortfolios = false;
        public const bool DefaultSendVqNotificationsForWatchPortfolios = false;
        public const bool DefaultCloseFractionalShares = false;
        public const bool DefaultAutoCreateStsAlerts = true;
        public const string DefaultTriggerTypeNameForOptions = "trailingStopsPercent";
        public const decimal DefaultTriggerPredefinedValueForOptions = 25;
        public const ExchangesFlags DefaultVisibleTradeStopsExchanges = ExchangesFlags.All & ~ExchangesFlags.CryptoCurrencies;
        public const ExchangesFlags DefaultVisibleCryptoStopsExchanges = ExchangesFlags.CryptoCurrencies;
        public const int DefaultSyncPortfoliosLimit = 50;
        public const int DefaultNewPositionPeriodInDays = 5;
        public const SsiFlags DefaultHealthSignalSettings = SsiFlags.Entry | SsiFlags.UpTrend | SsiFlags.NoTrend | SsiFlags.DownTrend | SsiFlags.StoppedOut;
        public const bool UseTransactionCommissions = false;
    }
}
