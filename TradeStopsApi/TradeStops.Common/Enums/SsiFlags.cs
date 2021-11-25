using System;

namespace TradeStops.Common.Enums
{
    [Flags]
    public enum SsiFlags
    {
        None = 0,

        Entry = 1,

        UpTrend = 1 << 1, // 2

        NoTrend = 1 << 2, // 4

        DownTrend = 1 << 3, // 8

        StoppedOut = 1 << 4 // 16
    }
}
