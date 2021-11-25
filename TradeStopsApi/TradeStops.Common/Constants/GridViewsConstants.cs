using TradeStops.Common.Enums;

namespace TradeStops.Common.Constants
{
    public static class GridViewsConstants
    {
        public const string DefaultViewName = "Default View";
        public const string FundamentalsViewName = "Fundamentals";

        public static ViewColumnTypes[] SsiColumns => new[]
        {
            ViewColumnTypes.Ssi,
            ViewColumnTypes.DaysInSsiState,
            ViewColumnTypes.DaysSinceSsiEntry,
            ViewColumnTypes.SsiAtRisk,
            ViewColumnTypes.SsiTriggerPrice, 
            ViewColumnTypes.SsiTrend
        };

        public static ViewColumnTypes[] TimingColumns => new[]
        {
            ViewColumnTypes.TimingTurnArea, 
            ViewColumnTypes.TimingConvictionLevel, 
            ViewColumnTypes.Rsi14
        };

        public static ViewColumnTypes[] StrategiesColumns => new[]
        {
            ViewColumnTypes.Strategies 
        };

        public static ViewColumnTypes[] RatingColumns => new[]
        {
            ViewColumnTypes.Rating
        };

        public static ViewColumnTypes[] OptionColumns => new[]
        {
            ViewColumnTypes.Ask,
            ViewColumnTypes.Bid,
            ViewColumnTypes.BidAskSpread,
            ViewColumnTypes.Delta,
            ViewColumnTypes.Gamma,
            ViewColumnTypes.ImpliedVolatility,
            ViewColumnTypes.ImpliedVolatilityPercentile,
            ViewColumnTypes.ImpliedVolatilityRank,
            ViewColumnTypes.Theta,
            ViewColumnTypes.Vega
        };
    }
}
