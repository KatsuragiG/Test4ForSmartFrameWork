namespace TradeStops.Contracts
{
    /// <summary>
    /// Health Signal settings
    /// </summary>
    public class HealthSignalSettingsContract
    {
        /// <summary>
        /// Trigger alert for the Entry Signal state.
        /// </summary>
        public bool TrackEntrySignal { get; set; }

        /// <summary>
        /// Trigger alert for the Up Trend state.
        /// </summary>
        public bool TrackUpTrend { get; set; }

        /// <summary>
        /// Trigger alert for the Side Trend state.
        /// </summary>
        public bool TrackSideTrend { get; set; }

        /// <summary>
        /// Trigger alert for the Down Trend state.
        /// </summary>
        public bool TrackDownTrend { get; set; }

        /// <summary>
        /// Trigger alert for the Stopped Out state.
        /// </summary>
        public bool TrackStoppedOut { get; set; }
    }
}
