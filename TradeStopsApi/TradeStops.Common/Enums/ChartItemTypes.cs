using System;

namespace TradeStops.Common.Enums
{
    /// <summary>
    /// Chart item name.
    /// </summary>
    public enum ChartItemTypes : byte
    {
        Ssi = 1,

        SsiLowRiskZone = 2,

        SsiStopLoss = 3,

        SsiTrend = 4,

        VqTrailingStop = 5,

        PercentTrailingStop = 6,

        FixedPrice = 7,

        EntryDate = 8,

        EntryPriceAdj = 9,

        EntrySignal = 10,

        Volume = 11,

        Price = 12,

        VqValues = 13,

        ExitDate = 14,

        ExitPrice = 15,

        PureQuantValue = 16,

        FixedValue = 17,

        LikeFolioValues = 18,

        GreenSsiPercent = 19,

        YellowSsiUpTrendPercent = 20,
        
        YellowSsiSideTrendPercent = 21,

        YellowSsiDownTrendPercent = 22,

        RedSsiPercent = 23,
        
        [Obsolete("Replaced by Timings")]
        CyclesValues = 24,

        [Obsolete("Replaced by Timings")]
        CycleAreasTopValues = 25,

        [Obsolete("Replaced by Timings")]
        CycleAreasBottomValues = 26,

        BullBearIndicator = 27,

        Rsi = 28,

        VolumeByPrice = 29,

        TimingsValues = 30,

        TimingTurnAreaPeakValues = 31,

        TimingTurnAreaValleyValues = 32,

        TimingsShortTermValues = 33,

        TimingTurnAreaShortTermPeakValues = 34,

        TimingTurnAreaShortTermValleyValues = 35,

        TimingsMiddleTermValues = 36,

        TimingTurnAreaMiddleTermPeakValues = 37,

        TimingTurnAreaMiddleTermValleyValues = 38,

        TimingsLongTermValues = 39,

        TimingTurnAreaLongTermPeakValues = 40,

        TimingTurnAreaLongTermValleyValues = 41,

        TimingsVeryLongTermValues = 42,

        TimingTurnAreaVeryLongTermPeakValues = 43,

        TimingTurnAreaVeryLongTermValleyValues = 44,

        Rating = 45,

        StrongBearishPercent = 46,

        BearishPercent = 47,

        NeutralPercent = 48,

        BullishPercent = 49,

        StrongBullishPercent = 50,

        UpTrendPercent = 51,

        DownTrendPercent = 52,

        SideTrendPercent = 53
    }
}
