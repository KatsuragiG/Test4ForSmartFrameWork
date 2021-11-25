using TradeStops.Common.Enums;

namespace TradeStops.Common.Constants
{
    public static class TriggerTypesWithSpecificParameters
    {
        public static TriggerTypes[] TriggerTypesWithRisk => new[]
        {
            TriggerTypes.TrailingStopPercent,
            TriggerTypes.VolatilityQuotinent,
        };

        public static TriggerTypes[] TriggerTypesWithTsPercent => new[]
        {
            TriggerTypes.TrailingStopPercent,
            TriggerTypes.VolatilityQuotinent,
            TriggerTypes.UnStockVolatilityQuotinent,
            TriggerTypes.UnStockTrailingStopPercent,
        };

        public static TriggerTypes[] TriggerTypesWithStartDateAndPrice => new[]
        {
            TriggerTypes.TrailingStopPercent,
            TriggerTypes.VolatilityQuotinent,
            TriggerTypes.UnStockVolatilityQuotinent,
            TriggerTypes.UnStockTrailingStopPercent
        };

        public static TriggerTypes[] TriggerTypesWithAllowedTypeChange => new[]
        {
            TriggerTypes.TrailingStopPercent,
            TriggerTypes.VolatilityQuotinent,
            TriggerTypes.UnStockVolatilityQuotinent,
            TriggerTypes.UnStockTrailingStopPercent
        };
    }
}
