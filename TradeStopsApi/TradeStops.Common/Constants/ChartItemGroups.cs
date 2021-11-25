using TradeStops.Common.Enums;

namespace TradeStops.Common.Constants
{
    public static class ChartItemGroups
    {
        public static ChartItemTypes[] TimingTurnAreaLines => new[]
        {
            ChartItemTypes.TimingTurnAreaPeakValues,
            ChartItemTypes.TimingTurnAreaValleyValues,
            ChartItemTypes.TimingTurnAreaShortTermPeakValues,
            ChartItemTypes.TimingTurnAreaShortTermValleyValues,
            ChartItemTypes.TimingTurnAreaMiddleTermPeakValues,
            ChartItemTypes.TimingTurnAreaMiddleTermValleyValues,
            ChartItemTypes.TimingTurnAreaLongTermPeakValues,
            ChartItemTypes.TimingTurnAreaLongTermValleyValues,
            ChartItemTypes.TimingTurnAreaVeryLongTermPeakValues,
            ChartItemTypes.TimingTurnAreaVeryLongTermValleyValues,
        };

        public static ChartItemTypes[] TimingPeakTurnAreaLines => new[]
        {
            ChartItemTypes.TimingTurnAreaPeakValues,
            ChartItemTypes.TimingTurnAreaShortTermPeakValues,
            ChartItemTypes.TimingTurnAreaMiddleTermPeakValues,
            ChartItemTypes.TimingTurnAreaLongTermPeakValues,
            ChartItemTypes.TimingTurnAreaVeryLongTermPeakValues,
        };

        public static ChartItemTypes[] TimingValueLines => new[]
        {
            ChartItemTypes.TimingsValues,
            ChartItemTypes.TimingsShortTermValues,
            ChartItemTypes.TimingsMiddleTermValues,
            ChartItemTypes.TimingsLongTermValues,
            ChartItemTypes.TimingsVeryLongTermValues,
        };
    }
}
