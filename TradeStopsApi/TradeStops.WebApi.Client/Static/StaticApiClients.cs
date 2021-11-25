using TradeStops.WebApi.Client.Generated;

namespace TradeStops.WebApi.Client.Static
{
    public static class StaticApiClients
    {
        public static IAdminPositionsClient AdminPositionsClient { get; private set; }
        public static IAdminSystemEventsClient AdminSystemEventsClient { get; private set; }
        public static IAdminAccountsClient AdminAccountsClient { get; private set; }
        public static IAdminAlertTemplatesClient AdminAlertTemplatesClient { get; private set; }
        public static IAdminNotificationAddressesClient AdminNotificationAddressesClient { get; private set; }
        public static IAdminUsersClient AdminUsersClient { get; private set; }
        public static IAdminPortfoliosClient AdminPortfoliosClient { get; private set; }
        public static IAdminUnconfirmedPositionsClient AdminUnconfirmedPositionsClient { get; private set; }
        public static IAdminSyncReportsClient AdminSyncReportsClient { get; private set; }
        public static INewslettersClient NewslettersClient { get; private set; }
        public static IBrokerPillsClient BrokerPillsClient { get; private set; }
        public static IChartsClient ChartsClient { get; private set; }
        public static IAlertTemplatesClient AlertTemplatesClient { get; private set; }
        public static IInvestmentStrategiesClient InvestmentStrategiesClient { get; private set; }
        public static IMarketOutlooksClient MarketOutlooksClient { get; private set; }
        public static ISectorsClient SectorsClient { get; private set; }
        public static IStockFinderClient StockFinderClient { get; private set; }
        public static IUnconfirmedPositionsClient UnconfirmedPositionsClient { get; private set; }
        public static IPositionTriggersClient PositionTriggersClient { get; private set; }
        public static IViewsClient ViewsClient { get; private set; }
        public static IResetPasswordClient ResetPasswordClient { get; private set; }
        public static IFinancialInstitutionsClient FinancialInstitutionsClient { get; private set; }
        public static IResearchClient ResearchClient { get; private set; }
        public static IPairTradesClient PairTradesClient { get; private set; }
        public static IPositionsClient PositionsClient { get; private set; }
        public static ISystemEventsClient SystemEventsClient { get; private set; }
        public static IPortfoliosClient PortfoliosClient { get; private set; }
        public static IZendeskAppsClient ZendeskAppsClient { get; private set; }
        public static ICurrenciesClient CurrenciesClient { get; private set; }
        public static IMobileOperatorsClient MobileOperatorsClient { get; private set; }
        public static INotificationAddressesClient NotificationAddressesClient { get; private set; }
        public static ITagsClient TagsClient { get; private set; }
        public static ICorporateActionsClient CorporateActionsClient { get; private set; }
        public static IUsersClient UsersClient { get; private set; }
        public static ICountriesClient CountriesClient { get; private set; }
        public static IPricesClient PricesClient { get; private set; }
        public static ISymbolsClient SymbolsClient { get; private set; }
        public static IAdminTradeSmithUsersClient AdminTradeSmithUsersClient { get; private set; }
        public static IAdminNewsletterSubscriptionsClient AdminNewsletterSubscriptionsClient { get; private set; }
        public static INewsClient NewsClient { get; private set; }
        public static IBehavioralDataClient BehavioralDataClient { get; private set; }
        public static IProductFeaturesClient ProductFeaturesClient { get; private set; }
        public static IAdminProductFeaturesClient AdminProductFeaturesClient { get; private set; }
        public static ITradeSmithProductsClient TradeSmithProductsClient { get; private set; }
        public static ISystemSettingsClient SystemSettingsClient { get; private set; }
        public static IImportProcessesClient ImportProcessesClient { get; private set; }
        public static ISyncPortfoliosClient SyncPortfoliosClient { get; private set; }
        public static IYodleeUserContextsClient YodleeUserContextsClient { get; private set; }
        public static IAdminYodleeUserContextsClient AdminYodleeUserContextsClient { get; private set; }
        public static IUserContextsClient UserContextsClient { get; private set; }
        public static IVendorSyncErrorsClient VendorSyncErrorsClient { get; private set; }
        public static IExternalNotificationsClient ExternalNotificationsClient { get; private set; }
        public static IMobileDevicesClient MobileDevicesClient { get; private set; }
        public static IAdminMobileDevicesClient AdminMobileDevicesClient { get; private set; }
        public static IAdminFinancialInstitutionsClient AdminFinancialInstitutionsClient { get; private set; }
        public static IAdminFinancialInstitutionRulesClient AdminFinancialInstitutionRulesClient { get; private set; }
        public static ITimingsClient TimingsClient { get; private set; }
        public static IPortfolioTrackerOrganizationsClient PortfolioTrackerOrganizationsClient { get; private set; }

        static StaticApiClients()
        {
            InitAllClients();

            // "Static constructor is guaranteed to be executed _immediately_ before
            // the first reference to a member of that class - either creation of instance
            // or own static method/property of class."
            // So, I assume that if nobody uses this class, than there won't be any initialization exception.
        }

        private static void InitAllClients()
        {
            AdminPositionsClient = new AdminPositionsClient();
            AdminSystemEventsClient = new AdminSystemEventsClient();
            AdminAccountsClient = new AdminAccountsClient();
            AdminAlertTemplatesClient = new AdminAlertTemplatesClient();
            AdminNotificationAddressesClient = new AdminNotificationAddressesClient();
            AdminUsersClient = new AdminUsersClient();
            AdminPortfoliosClient = new AdminPortfoliosClient();
            AdminUnconfirmedPositionsClient = new AdminUnconfirmedPositionsClient();
            AdminSyncReportsClient = new AdminSyncReportsClient();
            NewslettersClient = new NewslettersClient();
            BrokerPillsClient = new BrokerPillsClient();
            ChartsClient = new ChartsClient();
            AlertTemplatesClient = new AlertTemplatesClient();
            InvestmentStrategiesClient = new InvestmentStrategiesClient();
            MarketOutlooksClient = new MarketOutlooksClient();
            SectorsClient = new SectorsClient();
            StockFinderClient = new StockFinderClient();
            UnconfirmedPositionsClient = new UnconfirmedPositionsClient();
            PositionTriggersClient = new PositionTriggersClient();
            ViewsClient = new ViewsClient();
            ResetPasswordClient = new ResetPasswordClient();
            FinancialInstitutionsClient = new FinancialInstitutionsClient();
            ResearchClient = new ResearchClient();
            PairTradesClient = new PairTradesClient();
            PositionsClient = new PositionsClient();
            SystemEventsClient = new SystemEventsClient();
            PortfoliosClient = new PortfoliosClient();
            ZendeskAppsClient = new ZendeskAppsClient();
            CurrenciesClient = new CurrenciesClient();
            MobileOperatorsClient = new MobileOperatorsClient();
            NotificationAddressesClient = new NotificationAddressesClient();
            TagsClient = new TagsClient();
            CorporateActionsClient = new CorporateActionsClient();
            UsersClient = new UsersClient();
            CountriesClient = new CountriesClient();
            PricesClient = new PricesClient();
            SymbolsClient = new SymbolsClient();
            AdminTradeSmithUsersClient = new AdminTradeSmithUsersClient();
            AdminNewsletterSubscriptionsClient = new AdminNewsletterSubscriptionsClient();
            NewsClient = new NewsClient();
            BehavioralDataClient = new BehavioralDataClient();
            ProductFeaturesClient = new ProductFeaturesClient();
            TradeSmithProductsClient = new TradeSmithProductsClient();
            AdminProductFeaturesClient = new AdminProductFeaturesClient();
            SystemSettingsClient = new SystemSettingsClient();
            ImportProcessesClient = new ImportProcessesClient();
            SyncPortfoliosClient = new SyncPortfoliosClient();
            YodleeUserContextsClient = new YodleeUserContextsClient();
            AdminYodleeUserContextsClient = new AdminYodleeUserContextsClient();
            UserContextsClient = new UserContextsClient();
            VendorSyncErrorsClient = new VendorSyncErrorsClient();
            ExternalNotificationsClient = new ExternalNotificationsClient();
            MobileDevicesClient = new MobileDevicesClient();
            AdminMobileDevicesClient = new AdminMobileDevicesClient();
            AdminFinancialInstitutionsClient = new AdminFinancialInstitutionsClient();
            AdminFinancialInstitutionRulesClient = new AdminFinancialInstitutionRulesClient();
            TimingsClient = new TimingsClient();
            PortfolioTrackerOrganizationsClient = new PortfolioTrackerOrganizationsClient();
        }
    }
}