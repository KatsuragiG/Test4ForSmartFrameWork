using System;
using System.Diagnostics.CodeAnalysis;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class UserSettingsContract
    {
        public string AlertsLanguage { get; set; }

        public int MaxConsecutiveAlerts { get; set; }

        public int DaysBeforeAlertDelete { get; set; }

        public bool IgnoreDividend { get; set; }

        public string EntryPage { get; set; }

        public decimal EntryCommission { get; set; }

        public decimal ExitCommission { get; set; }

        public bool UseTransactionCommissions { get; set; }

        public string DefaultTab { get; set; }

        public bool RemoveAlertsOnceTriggered { get; set; }

        public bool EnableImportPortfolios { get; set; }

        public bool SendConsolidateEmail { get; set; }

        public bool SendNewsletterUpdates { get; set; }

        public bool SendBillionaireClubUpdates { get; set; }

        public bool SendDecoderUpdates { get; set; }

        public bool SendLikeFolioUpdates { get; set; }

        public bool SendVqNotificationsForInvestmentPortfolios { get; set; }

        public bool SendVqNotificationsForWatchPortfolios { get; set; }

        public bool SendCorporateActionNotificationsForInvestmentPortfolios { get; set; }

        public bool SendCorporateActionNotificationsForWatchPortfolios { get; set; }

        public bool SendStrategyUpdates { get; set; }

        public bool CloseFractionalShares { get; set; }

        public int CurrencyId { get; set; }

        public int AlertStartTime { get; set; }

        public int AlertEndTime { get; set; }

        public string TimeZoneId { get; set; }

        public bool AutoCreateReEntryAlerts { get; set; }

        /// <summary>
        /// Gets or sets alert (trigger) type name, if alert templates not are available for the user.
        /// </summary>
        public string TriggerTypeName { get; set; }

        /// <summary>
        /// Gets or sets alert (trigger) predefined value, if alert templates not are available for the user.
        /// </summary>
        public decimal? TriggerPredefinedValue { get; set; }

        /// <summary>
        /// Gets or sets alert (trigger) type name for options, if alert templates not are available for the user.
        /// </summary>
        public string TriggerTypeNameForOptions { get; set; }

        /// <summary>
        /// Gets or sets alert (trigger) predefined value for options, if alert templates not are available for the user.
        /// </summary>
        public decimal? TriggerPredefinedValueForOptions { get; set; }

        /// <summary>
        /// Gets or sets alert template for Pro (?) user for stocks.
        /// </summary>
        public int? AlertTemplateId { get; set; }

        /// <summary>
        /// Gets or sets alert template for Pro (?) user for options.
        /// </summary>
        public int? AlertTemplateIdForOptions { get; set; }

        public byte ExchangesFlags { get; set; }

        public ConfirmEmailFlowTypes ConfirmEmailFlowType { get; set; }

        public string UnconfirmedNotificationAddress { get; set; }

        public AlertsDefaultTabs AlertsDefaultTab { get; set; }

        public ResearchDefaultTabs ResearchDefaultTab { get; set; }

        public PortfoliosDefaultTabs PortfoliosDefaultTab { get; set; }

        public bool IsSendingUnconfirmedAddressDisabled { get; set; }

        public bool TransferNotesForReEntryPositions { get; set; }

        public bool GrantAccess { get; set; }

        public DateTime? AccessExpireAt { get; set; }

        public bool SyncErrorNoticesEnabled { get; set; }

        public ConsolidatedAlertTypes ConsolidatedAlertTypeId { get; set; }

        /// <summary>
        /// Indicates whether user wants to receive push notifications in Mobile app. 
        /// </summary>
        public bool SendPushNotifications { get; set; }

        /// <summary>
        /// Determines the grouping type (per alert, daily, by schedule) of push notifications in Mobile app.
        /// </summary>
        public ConsolidatedPushNotificationTypes ConsolidatedPushNotificationTypeId { get; set; }

        public bool CalculateEntryPriceFromCostBasis { get; set; }

        public bool CalculateEntryPriceWithCrossCourse { get; set; }

        /// <summary>
        /// Indicates whether user wants to receive notifications when Entry Signal is triggered for opened positinos in Investment portfolios. 
        /// </summary>
        public bool EntrySignalNotificationsForInvestmentPortfolios { get; set; }

        /// <summary>
        /// Indicates whether user wants to receive notifications when Entry Signal is triggered for opened positinos in Watch Only portfolios. 
        /// </summary>
        public bool EntrySignalNotificationsForWatchPortfolios { get; set; }

        /// <summary>
        /// Indicates whether user wants to receive notifications when Entry Signal is triggered for opened positinos in Newsletter portfolios. 
        /// </summary>
        public bool EntrySignalNotificationsForNewsletterPortfolios { get; set; }

        /// <summary>
        /// Indicates whether user wants to receive notifications when Early Entry Signal is triggered for opened positinos in Investment portfolios. 
        /// </summary>
        public bool EarlyEntrySignalNotificationsForInvestmentPortfolios { get; set; }

        /// <summary>
        /// Indicates whether user wants to receive notifications when Early Entry Signal is triggered for opened positinos in Watch Only portfolios. 
        /// </summary>
        public bool EarlyEntrySignalNotificationsForWatchPortfolios { get; set; }

        /// <summary>
        /// Indicates whether user wants to receive notifications when Early Entry Signal is triggered for opened positinos in Newsletter portfolios. 
        /// </summary>
        public bool EarlyEntrySignalNotificationsForNewsletterPortfolios { get; set; }

        /// <summary>
        /// Indicates whether user wants to receive notifications when New High Profit is triggered for opened positinos in Investment portfolios. 
        /// </summary>
        public bool NewHighProfitNotificationsForInvestmentPortfolios { get; set; }

        /// <summary>
        /// Indicates whether user wants to receive notifications when New High Profit is triggered for opened positinos in Watch Only portfolios. 
        /// </summary>
        public bool NewHighProfitNotificationsForWatchPortfolios { get; set; }

        /// <summary>
        /// Indicates whether user wants to receive notifications when New High Profit is triggered for opened positinos in Newsletter portfolios. 
        /// </summary>
        public bool NewHighProfitNotificationsForNewsletterPortfolios { get; set; }

        /// <summary>
        /// Indicates whether user wants to receive notifications when Health status changes for opened positinos in Investment portfolios. 
        /// </summary>
        public HealthSignalSettingsContract HealthSignalNotificationsForInvestmentPortfolios { get; set; }

        /// <summary>
        /// Indicates whether user wants to receive notifications when Health status changes for opened positinos in Watch Only portfolios. 
        /// </summary>
        public HealthSignalSettingsContract HealthSignalNotificationsForWatchPortfolios { get; set; }

        /// <summary>
        /// Indicates whether user wants to receive notifications when Health status changes for opened positinos in Newsletter portfolios. 
        /// </summary>
        public HealthSignalSettingsContract HealthSignalNotificationsForNewsletterPortfolios { get; set; }

        public bool IsAccessGranted => this.GrantAccess && (this.AccessExpireAt > DateTime.UtcNow || this.AccessExpireAt == null); // todo: assign property in api

        /// <summary>
        /// Indicates whether user wants to receive notifications when Stock Rating changes for opened positinos in Investment portfolios. 
        /// </summary>
        public bool StockRatingNotificationsForInvestmentPortfolios { get; set; }

        /// <summary>
        /// Indicates whether user wants to receive notifications when Stock Rating changes for opened positinos in Watch Only portfolios. 
        /// </summary>
        public bool StockRatingNotificationsForWatchPortfolios { get; set; }

        /// <summary>
        /// Indicates whether user wants to receive notifications when Stock Rating changes for opened positinos in Newsletter portfolios. 
        /// </summary>
        public bool StockRatingNotificationsForNewsletterPortfolios { get; set; }
    }
}
