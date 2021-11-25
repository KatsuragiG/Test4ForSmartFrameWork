using TradeStops.Common.Enums;

namespace TradeStops.Common.Constants
{
    public static class SmithFilterParameters
    {
        public const SsiStatuses SsiValue = SsiStatuses.Entry;

        public const decimal MinVolume = 0;

        public const decimal MaxAverage30YearsVq = 40;

        public const decimal MinAverageVolumeShares = 100000;

        public const decimal MinAverageVolumeValue = 2000000;
    }
}
