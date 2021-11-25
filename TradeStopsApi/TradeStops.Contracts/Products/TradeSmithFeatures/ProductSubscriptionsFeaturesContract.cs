using System.Diagnostics.CodeAnalysis;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Information about available features for one or many specified ProductSubscriptions.
    /// For multiple ProductSubscriptions it contains aggregated (summary) availability information.
    /// This contract contains all fields that we have in the ProductSubscriptionsFeatures db table
    /// except ProductSubscriptionId (because it can contain aggregated data)
    /// + some other non-database fields, that can be moved into database in future.
    /// It is intended to be used only internally by API and AdminArea.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements must be documented", Justification = "Internal. See UserFeaturesContract for details.")]
    public class ProductSubscriptionsFeaturesContract
    {
        // Products
        public bool AccessToPlatformProduct { get; set; }
        public bool AccessToMobileApp { get; set; }

        // Dashboard widgets
        public bool MarketOverviewWidget { get; set; }
        public bool QuickActionsWidget { get; set; }

        public bool MarketHealthWidget { get; set; }
        public bool MarketHealthMarketOutlookWidget { get; set; }
        public bool MarketHealthSpSectorsWidget { get; set; }
        public bool MarketHealthCommoditiesWidget { get; set; }

        public bool TradeSmithInsightsWidget { get; set; }
        public bool PortfolioOverviewWidget { get; set; }
        public bool PortfolioDistributionWidget { get; set; }
        public bool PortfolioEquityPerformanceWidget { get; set; }
        public bool WinnersLosersWidget { get; set; }
        public bool RecentEventsWidget { get; set; }
        public bool RecentNewslettersWidget { get; set; }
        public bool TrendingStocksWidget { get; set; }
        public bool InvestStrategiesWidget { get; set; }
        public bool RatingAllocationWidget { get; set; }
        public bool PossibleShortSqueezeWidget { get; set; }

        // Portfolio Management
        public int PortfoliosCount { get; set; }
        public int SyncPortfoliosCount { get; set; }
        public int AlertsCount { get; set; }

        public bool Positions { get; set; }

        public bool AddStocks { get; set; }
        public bool AddCrypto { get; set; }
        public bool AddOptions { get; set; }
        
        public bool ImportPortfolioFromCsv { get; set; }

        public bool Alerts { get; set; }

        public bool Templates { get; set; }

        public bool FundamentalsView { get; set; }

        // Alerts
        public bool PriceTargetAlerts { get; set; }
        public bool TimeBasedAlerts { get; set; }
        public bool VolumeAlerts { get; set; }
        public bool MovingAverageAlerts { get; set; }
        public bool TrailingStopsAlerts { get; set; }
        public bool OptionCostBasisAlerts { get; set; }
        public bool UnderlyingStockAlerts { get; set; }
        public bool TimeValueExpiryAlerts { get; set; }
        public bool FundamentalsAlerts { get; set; }
        public bool SsiAlerts { get; set; }

        // System alerts
        public bool EntrySignalAlerts { get; set; }

        // Ideas Lab
        public bool BestOfBillionairesStrategy { get; set; }
        public bool KineticVqStrategy { get; set; }
        public bool LowRiskRunnersStrategy { get; set; }
        public bool SectorSelectsStrategy { get; set; }
        public bool DividendGrowersStrategy { get; set; }
        public bool GrowthStrategy { get; set; }
        public bool ValueStrategy { get; set; }

        // Crypto Ideas Lab
        public bool CryptoKineticVqStrategy { get; set; }
        public bool CryptoLowRiskRunnersStrategy { get; set; }
        public bool CryptoMomentumPairingStrategy { get; set; }
        public bool CryptoMomentumAndVqPairingStrategy { get; set; }

        // Markets
        public bool MarketOutlook { get; set; }
        public bool SpSectors { get; set; }
        public bool Commodities { get; set; }

        public bool CryptoMarketOutlook { get; set; }

        // Tools
        public bool RiskRebalancer { get; set; }
        public bool AssetAllocation { get; set; }
        public bool PvqAnalyzer { get; set; }

        public bool PureQuant { get; set; }
        public bool PureQuantForStocks { get; set; }
        public bool PureQuantForCrypto { get; set; }

        public bool StockFinder { get; set; }
        public bool StockFinderForStocks { get; set; }
        public bool StockFinderForCrypto { get; set; }

        public bool StockAnalyzer { get; set; }

        // Position Size
        public bool PositionSizeDollarInvestmentRisk { get; set; }
        public bool PositionSizeRiskPercentageOfPortfolio { get; set; }
        public bool PositionSizeEqualPositionRisk { get; set; }
        public bool BulkPositionSize { get; set; }

        public bool Backtester { get; set; }

        // Newsletters
        public bool Newsletters { get; set; }
        public bool BillionairesPortfolio { get; set; }
        public bool AllNewsletters { get; set; }

        // Other
        public bool PositionCard { get; set; }
        public bool ChartAsImage { get; set; }
        public bool LikeFolio { get; set; }
        public bool SsiHealth { get; set; }
        public bool BullBear { get; set; }
        public bool Timing { get; set; }
        public bool OptionChain { get; set; }
        public bool GlobalStockRank { get; set; }
        public bool QuickSight { get; set; }
        public bool BestTradeOpportunitiesReport { get; set; }
        public bool BasketsManagement { get; set; }
        public bool ExpiringOptionsNotificationReport { get; set; }
        public bool SymbolGroupDistributions { get; set; }
        public bool SearchForTicker { get; set; }
        public bool PlatinumArea { get; set; }
        public bool CoPilotCompanion { get; set; }
        public bool CoPilotCompanionReport { get; set; }

        // Exchanges
        public bool CryptoExchange { get; set; }
        public bool StockExchanges { get; set; }

        // Portfolio Sync
        public bool ImportPortfolios { get; set; }
        public bool RefreshImportedPortfolios { get; set; }
        public bool SyncPortfolioForCrypto { get; set; }

        // Publications
        public bool InsideTradeSmithPublication { get; set; }
        public bool TradeSmithDailyPublication { get; set; }
        public bool TradeSmithDecoderPublication { get; set; }
        public bool TimingByTradeSmithPublication { get; set; }
        public bool TradeSmithTrendsPublication { get; set; }
        public bool LikeFolioMegatrendsPublication { get; set; }
        public bool LikeFolioOpportunityAlertsPublication { get; set; }
        public bool EarningsReportsPublication { get; set; }
        public bool MarketWeeklyUpdatePublication { get; set; }
        public bool MoneyTalksPublication { get; set; }
        public bool PlatinumOnlyAnnouncementsPublication { get; set; }
        public bool PlatinumBenefitsPublication { get; set; }

        // PortfolioTracker
        public bool PortfolioTracker { get; set; }

        // Gurus
        public bool GurusOverview { get; set; }
        public bool GurusPortfolioEquityPerformanceWidget { get; set; }
        public bool GurusTopRecommendations { get; set; }

        // Portfolio Lite
        public bool PortfolioLite { get; set; }

        // Baskets
        public bool AllCryptocurrenciesBasket { get; set; }
        public bool Cci30Basket { get; set; }
        public bool BinanceBasket { get; set; }
        public bool BinanceUsBasket { get; set; }
        public bool CoinbaseBasket { get; set; }
        public bool PoloniexBasket { get; set; }
        public bool MarketsBasket { get; set; }
        public bool CopilotCompanionBasket { get; set; }
        public bool PossibleShortSqueezeBasket { get; set; }
    }
}
