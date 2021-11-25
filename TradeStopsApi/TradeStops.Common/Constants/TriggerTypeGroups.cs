using TradeStops.Common.Enums;

namespace TradeStops.Common.Constants
{
    public static class TriggerTypeGroups
    {
        public static TriggerTypes[] Volume => new[]
        {
            TriggerTypes.PercentOfAverageVolume,
        };

        public static TriggerTypes[] Time => new[]
        {
            TriggerTypes.CalendarDays,
            TriggerTypes.TradingDays,
            TriggerTypes.ProfitableCloses,
            TriggerTypes.SpecificDate,
        };

        public static TriggerTypes[] Price => new[]
        {
            TriggerTypes.TwoVolatilityQuotient,
            TriggerTypes.Breakout, // Time group in Database (groupId = 1)
            TriggerTypes.PercentageGainLoss,
            TriggerTypes.FixedPrice,
            TriggerTypes.DollarGainLoss
        };

        public static TriggerTypes[] PriceForOptions => new[]
        {
            TriggerTypes.Breakout, // Time group in Database (groupId = 1)
            TriggerTypes.PercentageGainLoss,
            TriggerTypes.FixedPrice,
            TriggerTypes.DollarGainLoss
        };

        public static TriggerTypes[] PriceForPairTrades => new[]
        {
            TriggerTypes.TwoVolatilityQuotient,
            TriggerTypes.PercentageGainLoss
        };

        public static TriggerTypes[] TrailingStop => new[]
        {
            TriggerTypes.TrailingStopPercent,
            TriggerTypes.VolatilityQuotinent
        };

        public static TriggerTypes[] MovingAverage => new[]
        {
            TriggerTypes.MovingAveragePrice, // VolumeAndMovingAverage group in database (Volume group here, groupId = 1)
            TriggerTypes.MovingAverageCrosses, // VolumeAndMovingAverage group in database (Volume group here, groupId = 1)
        };

        public static TriggerTypes[] OptionCostBasis => new[]
        {
            TriggerTypes.NakedPut,
            TriggerTypes.CoveredCall,
        };

        public static TriggerTypes[] UnderlyingStock => new[]
        {
            TriggerTypes.UnStockFixedPrice,
            TriggerTypes.UnStockVolatilityQuotinent,
            TriggerTypes.UnStockTrailingStopPercent,
        };

        public static TriggerTypes[] TimeValueExpiry => new[]
        {
            TriggerTypes.PercentageTimeValue,
            TriggerTypes.DollarTimeValue,
            TriggerTypes.Expiry
        };

        public static TriggerTypes[] Fundamentals => new[]
        {
            TriggerTypes.Target,
        };
    }
}
