using System.Diagnostics.CodeAnalysis;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Values to customize features for specified user.
    /// These permissions are applied additionally to subscription permissions,
    /// and remain unmodified even when subscription level is changed.
    /// So, if somebody enables any feature in Admin Area,
    /// then it will be enabled until somebody will disable it explicitly.
    /// This contract is the exact copy of the values that we have in the database.
    /// It is intended to be used only internally by API and AdminArea.
    /// Not null for existing user.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements must be documented", Justification = "Internal. See UserFeaturesContract for details.")]
    public class UserFeaturesCustomizationContract
    {
        // Dashboard widgets
        public bool MarketOverviewWidget { get; set; }
        public bool QuickActionsWidget { get; set; }
        public bool MarketHealthWidget { get; set; }
        public bool TradeSmithInsightsWidget { get; set; }
        public bool PortfolioOverviewWidget { get; set; }
        public bool PortfolioDistributionWidget { get; set; }
        public bool PortfolioEquityPerformanceWidget { get; set; }
        public bool WinnersLosersWidget { get; set; }
        public bool RecentEventsWidget { get; set; }
        public bool RecentNewslettersWidget { get; set; }
        public bool TrendingStocksWidget { get; set; }

        // Portfolio Management
        public int PortfoliosCount { get; set; }
        public int SyncPortfoliosCount { get; set; }
        public int AlertsCount { get; set; }

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
        public bool PvqAnalyzer { get; set; } // Ex-VqAllocation
        public bool PureQuant { get; set; } // Ex-QuantTool
        public bool StockFinder { get; set; }
        public bool StockAnalyzer { get; set; }

        public bool PositionSizeDollarInvestmentRisk { get; set; } // Ex-PositionSize
        public bool PositionSizeRiskPercentageOfPortfolio { get; set; } // Ex-PositionSize
        public bool PositionSizeEqualPositionRisk { get; set; } // Ex-PositionSize

        public bool Backtester { get; set; }

        // Newsletters
        public bool Newsletters { get; set; }
        public bool BillionairesPortfolio { get; set; }

        // Other
        public bool PositionCard { get; set; }
        public bool ChartAsImage { get; set; }
        public bool LikeFolio { get; set; }
        public bool SsiHealth { get; set; }
        public bool BullBear { get; set; }
        public bool Timing { get; set; }
        public bool OptionChain { get; set; }
        public bool QuickSight { get; set; }
        public bool OptionSpreads { get; set; }
        public bool SearchForTicker { get; set; }
        public bool PlatinumArea { get; set; }
        public bool CoPilotCompanion { get; set; }
    }
}
