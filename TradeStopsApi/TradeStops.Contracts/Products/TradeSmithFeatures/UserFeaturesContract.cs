using System;
using System.Diagnostics.CodeAnalysis;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Information about all features available for user.
    /// Includes features allowed by active ProductSubscriptions
    /// and features allowed in UserFeaturesCustomization.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements must be documented", Justification = "Temporary suppression")]
    public class UserFeaturesContract
    {
        /// <summary>
        /// ID of the user.
        /// </summary>
        public int TradeSmithUserId { get; set; }

        /// <summary>
        /// Indicates whether user has any active subscription for Platform product
        /// </summary>
        public bool AccessToPlatformProduct { get; set; }

        /// <summary>
        /// Indicates whether user has access to mobile app or not.
        /// </summary>
        public bool AccessToMobileApp { get; set; }

        /// <summary>
        /// Availability of dashboard widgets.
        /// </summary>
        public AvailableWidgetsContract Widgets { get; set; }

        /// <summary>
        /// Availability of different groups of Alerts.
        /// </summary>
        public AvailableAlertsContract Alerts { get; set; }

        /// <summary>
        /// Availability of TradeIdeas Strategies, including Crypto-strategies.
        /// </summary>
        public AvailableStrategiesContract Strategies { get; set; }

        /// <summary>
        /// Availability of the tools like AssetAllocation, PureQuant, etc.
        /// </summary>
        public AvailableToolsContract Tools { get; set; }

        /// <summary>
        /// Availability of publication types
        /// </summary>
        public AvailablePublicationsContract Publications { get; set; }

        /// <summary>
        /// Availability of baskets.
        /// Each basket is a set of symbols combined under one name.
        /// </summary>
        public AvailableBasketsContract Baskets { get; set; }

        /// <summary>
        /// Portfolio Management: Maximum number of portfolios that user can create
        /// (portfolios count by subscription + additional portfolios customization).
        /// </summary>
        public int PortfoliosCount { get; set; }

        /// <summary>
        /// Portfolio Management: Number of portfolios that can be imported from single brokerage account with one import.
        /// This value doesn't limit number of synchronized portfolios in user's account - we have PortfoliosCount for both manual and synchronized portfolios.
        /// </summary>
        public int SyncPortfoliosCount { get; set; }

        /// <summary>
        /// Portfolio Management: Maximum number of alerts that user can create
        /// (alerts count by subscription + additional alerts customization).
        /// </summary>
        public int AlertsCount { get; set; }

        /// <summary>
        /// Portfolio Management: Indicates availability of positions for user.
        /// Used by Nightly Update to decide if it's necessary to process user's positions.
        /// This property has similar meaning to PortfoliosCount property,
        /// but it's preferable to use because it's boolean property (it's simpler).
        /// </summary>
        public bool Positions { get; set; }

        /// <summary>
        /// Portfolio Management: Permission to add stock positions into portfolios.
        /// </summary>
        public bool AddStocks { get; set; }

        /// <summary>
        /// Portfolio Management: Permission to add crypto positions into portfolios.
        /// </summary>
        public bool AddCrypto { get; set; }

        /// <summary>
        /// Portfolio Management: Permission to add options into portfolios.
        /// </summary>
        public bool AddOptions { get; set; }

        /// <summary>
        /// Portfolio Management: Permission to import portfolio from CSV file.
        /// </summary>
        public bool ImportPortfolioFromCsv { get; set; }

        /// <summary>
        /// Indicates availability of alert templates.
        /// </summary>
        public bool Templates { get; set; }

        /// <summary>
        /// Indicates availability of 'Fundamentals' grid view.
        /// </summary>
        public bool FundamentalsView { get; set; }

        /// <summary>
        /// Markets: Indicates availability of Market Outlook page.
        /// </summary>
        public bool MarketOutlook { get; set; }

        /// <summary>
        /// Markets: Indicates availability of SP500 Sectors page.
        /// </summary>
        public bool SpSectors { get; set; }

        /// <summary>
        /// Markets: Indicates availability of Commodities page.
        /// </summary>
        public bool Commodities { get; set; }

        /// <summary>
        /// Markets: Indicates availability of Crypto Market Outlook page.
        /// </summary>
        public bool CryptoMarketOutlook { get; set; }

        /// <summary>
        /// Newsletters: Indicates availability of Newsletters page.
        /// </summary>
        public bool Newsletters { get; set; }

        /// <summary>
        /// Newsletters: Indicates availability of Billionaires portfolio.
        /// </summary>
        public bool BillionairesPortfolio { get; set; }

        /// <summary>
        /// Flag indicates that all possible newsletters must be available for user.
        /// If this flag equals true, all other newsletter flags will also be equal true.
        /// </summary>
        public bool AllNewsletters { get; set; }

        // Other
        public bool PositionCard { get; set; }
        public bool ChartAsImage { get; set; }

        /// <summary>
        /// This feature is responsible for availability of LikeFolio data,
        /// that is stored in ResearchData database, LikeFolioValues table.
        /// For example, on website this data is displayed in LikeFolio pills on Position Card and StockAnalyzer
        /// </summary>
        public bool LikeFolio { get; set; }

        public bool SsiHealth { get; set; }
        public bool BullBear { get; set; }

        /// <summary>
        /// This feature is responsible for availability of Best Trade Opportunities report
        /// Used to get a list of users to send Best Trade Opportunities report.
        /// </summary>
        public bool BestTradeOpportunitiesReport { get; set; }

        /// <summary>
        /// This feature is responsible for availability of Baskets Management.
        /// Check system baskets availability in AvailableBasketsContract.
        /// </summary>
        public bool BasketsManagement { get; set; }

        /// <summary>
        /// This feature is responsible for availability of Expiring Options Notification report
        /// Used to get a list of users to send Expiring Options Notification report.
        /// </summary>
        public bool ExpiringOptionsNotificationReport { get; set; }

        /// <summary>
        /// Determines availability of distribution tab for stock analyzer and position card pages.
        /// </summary>
        public bool SymbolGroupDistributions { get; set; }

        public bool SearchForTicker { get; set; }

        /// <summary>
        /// Determines availability of RSI and Volume at Risk features
        /// </summary>
        public bool Timing { get; set; }

        /// <summary>
        /// Determines availability of option chain.
        /// Option Chain is used in Finance website for Options tab on PositionCard and on StockAnalyzer page.
        /// </summary>
        public bool OptionChain { get; set; }

        /// <summary>
        /// Determines availability of "Explore more" link to Billionaires Club page
        /// </summary>
        public bool QuickSight { get; set; }

        /// <summary>
        /// Determines availability of alternative tab on StockAnalyzer page.
        /// </summary>
        public bool OptionSpreads { get; set; }

        /// <summary>
        /// Determines availability of Global Stock Rank (Stock Rating, 'Speedometer') feature.
        /// </summary>
        public bool GlobalStockRank { get; set; }

        /// <summary>
        /// Determines whether user has access to crypto exchange or not.
        /// Used in UserSettings.ExchangesFlags + symbols autocomplete.
        /// </summary>
        public bool CryptoExchange { get; set; }

        /// <summary>
        /// Determines whether user has access to stock exchanges or not.
        /// Used in UserSettings.ExchangesFlags + symbols autocomplete.
        /// </summary>
        public bool StockExchanges { get; set; }

        /// <summary>
        /// Determines if importing portfolio from Yodlee/Plaid is available for user.
        /// </summary>
        public bool ImportPortfolios { get; set; }

        /// <summary>
        /// Determines if refreshing portfolio that was imported from Yodlee/Plaid is available for user.
        /// This feature is necessary mostly because of Portfolio Analyzer,
        /// where we allow to import portfolio, but we don't synchronize it unless the user buys paid subscription.
        /// </summary>
        public bool RefreshImportedPortfolios { get; set; }

        /// <summary>
        /// Determines if the user can import/update crypto positions. If not available items are imported as unconfirmed. 
        /// Moved out as a separate feature, because users with a Crypto Ideas subscription (CryptoFinder) are not able to import/refresh crypto positions. I do not know why.
        /// This is true for all users with any crypto subscription except Crypto Ideas. Nothing else is checked.
        /// </summary>
        [Obsolete("Use AddCrypto property instead.")]
        public bool SyncPortfolioForCrypto { get; set; }

        /// <summary>
        /// Availability of PortfolioTracker section in Finance website.
        /// </summary>
        public bool PortfolioTracker { get; set; }

        /// <summary>
        /// Indicates availability of Gurus Overview tab in Finance website.
        /// </summary>
        public bool GurusOverview { get; set; }

        /// <summary>
        /// Indicates availability of Portfolio Equity Performance widget on Gurus Overview tab in Finance website.
        /// </summary>
        public bool GurusPortfolioEquityPerformanceWidget { get; set; }
        
        /// <summary>
        /// Indicates availability of Top Recommendations tab in Gurus section in Finance website.
        /// </summary>
        public bool GurusTopRecommendations { get; set; }

        /// <summary>
        /// Availability of Portfolio Lite.
        /// </summary>
        public bool PortfolioLite { get; set; }

        /// <summary>
        /// Indicates availability of Platinum page.
        /// </summary>
        public bool PlatinumArea { get; set; }

        /// <summary>
        /// Indicates availability of CoPilot Companion page.
        /// </summary>
        public bool CoPilotCompanion { get; set; }

        /// <summary>
        /// This feature is responsible for availability of CoPilot Companion Report
        /// </summary>
        public bool CoPilotCompanionReport { get; set; }
    }
}
