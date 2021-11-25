using System;

namespace TradeStops.Common.Enums
{
    [Flags]
    public enum UserFlags
    {
        None = 0,

        SmartTrailingStopVideo = 1 << 0,

        PositionSizeCalculatorVideo = 1 << 2,

        AssetAllocationVideo = 1 << 3,

        VolatilityQuotientVideo = 1 << 4,

        NewsLettersVideo = 1 << 5,

        UpgradeOfStopLossAnalyzerAlert = 1 << 6,

        ////DontShowFaceliftAnnouncement = 1 << 7,

        ShowBeaconForCharts = 1 << 8,

        TermsOfService = 1 << 9,

        ShowReactivatedExpiredUsersPopup = 1 << 11,

        // Deprecated For TS
        ShowHelpCenterPopup = 1 << 12,

        ShowBeaconForPortfolios = 1 << 14
    }
}
