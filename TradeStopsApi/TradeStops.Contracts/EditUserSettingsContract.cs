using System;
using System.Diagnostics.CodeAnalysis;
using TradeStops.Common.Enums;
using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class EditUserSettingsContract
    {
        public Optional<string> AlertsLanguage { get; set; }

        public Optional<int> MaxConsecutiveAlerts { get; set; }

        public Optional<int> DaysBeforeAlertDelete { get; set; }

        public Optional<bool> IgnoreDividend { get; set; }

        public Optional<string> EntryPage { get; set; }

        public Optional<decimal> EntryCommission { get; set; }

        public Optional<decimal> ExitCommission { get; set; }

        public Optional<string> DefaultTab { get; set; }

        public Optional<bool> RemoveAlertsOnceTriggered { get; set; }

        public Optional<bool> EnableImportPortfolios { get; set; }

        public Optional<bool> SendConsolidateEmail { get; set; }

        public Optional<bool> SendNewsletterUpdates { get; set; }

        public Optional<bool> SendBillionaireClubUpdates { get; set; }

        public Optional<bool> SendDecoderUpdates { get; set; }

        public Optional<bool> SendLikeFolioUpdates { get; set; }

        public Optional<bool> SendVqNotificationsForInvestmentPortfolios { get; set; }

        public Optional<bool> SendVqNotificationsForWatchPortfolios { get; set; }

        public Optional<bool> SendCorporateActionNotificationsForInvestmentPortfolios { get; set; }

        public Optional<bool> SendCorporateActionNotificationsForWatchPortfolios { get; set; }

        public Optional<bool> SendStrategyUpdates { get; set; }

        public Optional<bool> CloseFractionalShares { get; set; }

        public Optional<int> CurrencyId { get; set; }

        public Optional<int> AlertStartTime { get; set; }

        public Optional<int> AlertEndTime { get; set; }

        public Optional<string> TimeZoneId { get; set; }

        public Optional<bool> AutoCreateReEntryAlerts { get; set; }

        /// <summary>
        /// (optional) Gets or sets alert (trigger) type name, if alert templates not are available for the user.
        /// </summary>
        public Optional<string> TriggerTypeName { get; set; }

        /// <summary>
        /// (optional) Gets or sets alert (trigger) predefined value, if alert templates not are available for the user.
        /// </summary>
        public Optional<decimal?> TriggerPredefinedValue { get; set; }

        /// <summary>
        /// (optional) Gets or sets alert (trigger) type name for options, if alert templates not are available for the user.
        /// </summary>
        public Optional<string> TriggerTypeNameForOptions { get; set; }

        /// <summary>
        /// (optional) Gets or sets alert (trigger) predefined value for options, if alert templates not are available for the user.
        /// </summary>
        public Optional<decimal?> TriggerPredefinedValueForOptions { get; set; }

        /// <summary>
        /// (optional) Gets or sets alert template for Pro (?) user for stocks.
        /// </summary>
        public Optional<int?> AlertTemplateId { get; set; }

        /// <summary>
        /// (optional) Gets or sets alert template for Pro (?) user for options.
        /// </summary>
        public Optional<int?> AlertTemplateIdForOptions { get; set; }

        public Optional<byte> ExchangesFlags { get; set; }

        public Optional<ConfirmEmailFlowTypes> ConfirmEmailFlowType { get; set; }

        public Optional<string> UnconfirmedNotificationAddress { get; set; }

        public Optional<AlertsDefaultTabs> AlertsDefaultTab { get; set; }

        public Optional<ResearchDefaultTabs> ResearchDefaultTab { get; set; }

        public Optional<PortfoliosDefaultTabs> PortfoliosDefaultTab { get; set; }

        public Optional<bool> IsSendingUnconfirmedAddressDisabled { get; set; }

        public Optional<bool> TransferNotesForReEntryPositions { get; set; }

        public Optional<bool> GrantAccess { get; set; }

        public Optional<DateTime?> AccessExpireAt { get; set; }

        public Optional<bool> SyncErrorNoticesEnabled { get; set; }

        public Optional<ConsolidatedAlertTypes> ConsolidatedAlertTypeId { get; set; }

        /// <summary>
        /// (optional) Indicates whether user wants to receive push notifications in Mobile app. 
        /// </summary>
        public Optional<bool> SendPushNotifications { get; set; }

        /// <summary>
        /// (optional) Determines the grouping type (per alert, daily, by schedule) of push notifications in Mobile app.
        /// </summary>
        public Optional<ConsolidatedPushNotificationTypes> ConsolidatedPushNotificationTypeId { get; set; }

        public Optional<bool> CalculateEntryPriceFromCostBasis { get; set; }

        public Optional<bool>  CalculateEntryPriceWithCrossCourse { get; set; }

        /// <summary>
        /// (optional) Indicates whether user wants to receive notifications when Entry Signal is triggered for opened positinos in Investment portfolios. 
        /// </summary>
        public Optional<bool> EntrySignalNotificationsForInvestmentPortfolios { get; set; }

        /// <summary>
        /// (optional) Indicates whether user wants to receive notifications when Entry Signal is triggered for opened positinos in Watch Only portfolios. 
        /// </summary>
        public Optional<bool> EntrySignalNotificationsForWatchPortfolios { get; set; }

        /// <summary>
        /// (optional) Indicates whether user wants to receive notifications when Entry Signal is triggered for opened positinos in Newsletter portfolios. 
        /// </summary>
        public Optional<bool> EntrySignalNotificationsForNewsletterPortfolios { get; set; }

        /// <summary>
        /// (optional) Indicates whether user wants to receive notifications when Early Entry Signal is triggered for opened positinos in Investment portfolios. 
        /// </summary>
        public Optional<bool> EarlyEntrySignalNotificationsForInvestmentPortfolios { get; set; }

        /// <summary>
        /// (optional) Indicates whether user wants to receive notifications when Early Entry Signal is triggered for opened positinos in Watch Only portfolios. 
        /// </summary>
        public Optional<bool> EarlyEntrySignalNotificationsForWatchPortfolios { get; set; }

        /// <summary>
        /// (optional) Indicates whether user wants to receive notifications when Early Entry Signal is triggered for opened positinos in Newsletter portfolios. 
        /// </summary>
        public Optional<bool> EarlyEntrySignalNotificationsForNewsletterPortfolios { get; set; }

        /// <summary>
        /// (optional) Indicates whether user wants to receive notifications when New High Profit is triggered for opened positinos in Investment portfolios. 
        /// </summary>
        public Optional<bool> NewHighProfitNotificationsForInvestmentPortfolios { get; set; }

        /// <summary>
        /// (optional) Indicates whether user wants to receive notifications when New High Profit is triggered for opened positinos in Watch Only portfolios. 
        /// </summary>
        public Optional<bool> NewHighProfitNotificationsForWatchPortfolios { get; set; }

        /// <summary>
        /// (optional) Indicates whether user wants to receive notifications when New High Profit is triggered for opened positinos in Newsletter portfolios. 
        /// </summary>
        public Optional<bool> NewHighProfitNotificationsForNewsletterPortfolios { get; set; }

        /// <summary>
        /// (optional) Indicates whether user wants to receive notifications when Health status changes for opened positinos in Investment portfolios. 
        /// </summary>
        public Optional<HealthSignalSettingsContract> HealthSignalNotificationsForInvestmentPortfolios { get; set; }

        /// <summary>
        /// (optional) Indicates whether user wants to receive notifications when Health status changes for opened positinos in Watch Only portfolios. 
        /// </summary>
        public Optional<HealthSignalSettingsContract> HealthSignalNotificationsForWatchPortfolios { get; set; }

        /// <summary>
        /// (optional) Indicates whether user wants to receive notifications when Health status changes for opened positinos in Newsletter portfolios. 
        /// </summary>
        public Optional<HealthSignalSettingsContract> HealthSignalNotificationsForNewsletterPortfolios { get; set; }

        /// <summary>
        /// (optional) Indicates whether user wants to receive notifications when Stock Rating changes for opened positinos in Investment portfolios. 
        /// </summary>
        public Optional<bool> StockRatingNotificationsForInvestmentPortfolios { get; set; }

        /// <summary>
        /// (optional) Indicates whether user wants to receive notifications when Stock Rating changes for opened positinos in Watch Only portfolios. 
        /// </summary>
        public Optional<bool> StockRatingNotificationsForWatchPortfolios { get; set; }

        /// <summary>
        /// (optional) Indicates whether user wants to receive notifications when Stock Rating changes for opened positinos in Newsletter portfolios. 
        /// </summary>
        public Optional<bool> StockRatingNotificationsForNewsletterPortfolios { get; set; }
    }
}
