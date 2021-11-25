using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Timing Turn Area values
    /// </summary>
    public class TimingTurnAreaContract
    {
        /// <summary>
        /// ID of the turn area
        /// </summary>
        public int TimingTurnAreaId { get; set; }

        /// <summary>
        /// Timing ID
        /// </summary>
        public int TimingId { get; set; }

        /// <summary>
        /// Start date of area
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// End date of area
        /// </summary>
        public DateTime ToDate { get; set; }

        /// <summary>
        /// Type of area
        /// </summary>
        public TimingTurnAreaTypes TurnAreaType { get; set; }

        /// <summary>
        /// Turning area strength
        /// </summary>
        public TimingTurnStrengthTypes TurnStrength { get; set; }

        /// <summary>
        /// Type of serie
        /// </summary>
        public TimingSerieTypes SerieType { get; set; }
    }
}
