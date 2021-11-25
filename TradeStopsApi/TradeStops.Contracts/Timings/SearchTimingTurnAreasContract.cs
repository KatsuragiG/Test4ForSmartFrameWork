using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to get timing turn areas
    /// </summary>
    public class SearchTimingTurnAreasContract
    {
        /// <summary>
        /// IDs of specific timings to get
        /// </summary>
        public List<int> TimingIds { get; set; }

        /// <summary>
        /// (optional) Turn area types to get timings
        /// </summary>
        public List<TimingTurnAreaTypes> TurnAreaTypes { get; set; }

        /// <summary>
        /// (optional) Turn strength types to get timings
        /// </summary>
        public List<TimingTurnStrengthTypes> TurnStrengthTypes { get; set; }

        /// <summary>
        /// (optional) Turn serie types to get timings
        /// </summary>
        public List<TimingSerieTypes> SerieTypes { get; set; }
    }
}
