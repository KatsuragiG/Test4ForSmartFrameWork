namespace TradeStops.Contracts
{
    /// <summary>
    /// Information about available widgets in Platform (Finance) website
    /// </summary>
    public class AvailableWidgetsContract
    {
        /// <summary>
        /// Indexes Carousel (Market Outlook Slider)
        /// </summary>
        public bool MarketOverview { get; set; }

        /// <summary>
        /// The block with the links to different pages of the Platform website
        /// </summary>
        public bool QuickActions { get; set; }

        /// <summary>
        /// Widget with the tabs: Market Outlook, SP Sectors, Commodities
        /// </summary>
        public bool MarketHealth { get; set; }

        /// <summary>
        /// Market Outlook tab for Market Health widget
        /// </summary>
        public bool MarketHealthMarketOutlook { get; set; }

        /// <summary>
        /// SP Sectors tab for Market Health widget
        /// </summary>
        public bool MarketHealthSpSectors { get; set; }

        /// <summary>
        /// Commodities tab for Market Health widget
        /// </summary>
        public bool MarketHealthCommodities { get; set; }

        /// <summary>
        /// Widget with the recent publications from some external sources.
        /// </summary>
        public bool TradeSmithInsights { get; set; }

        /// <summary>
        /// Widget that is intended to display some general information for selected portfolio
        /// </summary>
        public bool PortfolioOverview { get; set; }

        /// <summary>
        /// Widget that displays portfolio distribution by the following options: Health (ex-SSI), Asset Allocation, Risk (VQ)
        /// </summary>
        public bool PortfolioDistribution { get; set; }

        /// <summary>
        /// Widget that compares portfolio performance (gain?) with other portfolios or major indexes performance on the chart.
        /// </summary>
        public bool PortfolioEquityPerformance { get; set; }

        /// <summary>
        /// Widget with top winners and losers symbols by gain.
        /// </summary>
        public bool WinnersLosers { get; set; }

        /// <summary>
        /// Widget with the information about recent system events like triggered alerts, purchased and sold positions.
        /// </summary>
        public bool RecentEvents { get; set; }

        /// <summary>
        /// Widget with recent newsletters recommendations
        /// </summary>
        public bool RecentNewsletters { get; set; }

        /// <summary>
        /// Widget with invest strategies.
        /// </summary>
        public bool InvestStrategies { get; set; }

        /// <summary>
        /// Widget with trending stocks and crypto
        /// </summary>
        public bool TrendingStocks { get; set; }

        /// <summary>
        /// Widget with global rank allocation.
        /// </summary>
        public bool RatingAllocation { get; set; }

        /// <summary>
        /// Widget with short interest
        /// </summary>
        public bool PossibleShortSqueeze { get; set; }
    }
}
