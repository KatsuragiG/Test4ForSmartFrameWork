using TradeStops.Common.Enums;
using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to edit portfolio tracker position
    /// </summary>
    public class EditPortfolioTrackerPositionContract
    {
        /// <summary>
        /// (Optional) Position type.
        /// </summary>
        public Optional<PositionTypes> PositionType { get; set; }

        /// <summary>
        /// (Optional) Recommended advice type for position.
        /// </summary>
        public Optional<PositionAdviceTypes> AdviceType { get; set; }

        /// <summary>
        /// (Optional) Additional notes for recommender advice.
        /// </summary>
        public Optional<string> AdviceNotes { get; set; }

        /// <summary>
        /// (Optional) First field for additional notes for position.
        /// </summary>
        public Optional<string> Notes1 { get; set; }

        /// <summary>
        /// (Optional) Indicates whether this position is published in TradeSmith Gurus center or not.
        /// </summary>
        public Optional<bool> Published { get; set; }
    }
}
