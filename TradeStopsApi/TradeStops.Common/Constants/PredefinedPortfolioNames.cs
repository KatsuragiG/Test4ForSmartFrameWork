namespace TradeStops.Common.Constants
{
    public static class PredefinedPortfolioNames
    {
        public const string BestOfBillionaires = "Best of the Billionaires";
        public const string PureQuant = "Pure Quant";
        public const string TradeSmithTrendingTickers = "TradeSmith Trending Tickers";
        public const string RecoveryPortfolio = "The Recovery Portfolio";

        public static string[] AllNames => new[]
        {
            BestOfBillionaires, PureQuant, TradeSmithTrendingTickers, RecoveryPortfolio
        };
    }
}
