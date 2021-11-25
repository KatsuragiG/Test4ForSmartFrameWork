namespace TradeStops.Common.Enums
{
    public enum PlatformTaskProgressTypes
    {
        // Pure Quant progress types
        PureQuantPending = 100,
        PureQuantPreparingSources = 101,
        PureQuantApplyingFilters = 102,
        PureQuantApplyingDiversification = 103,
        PureQuantCalculatingPositionSizes = 104,
        PureQuantRankingPositions = 105,
        PureQuantBuildFinished = 106
    }
}
