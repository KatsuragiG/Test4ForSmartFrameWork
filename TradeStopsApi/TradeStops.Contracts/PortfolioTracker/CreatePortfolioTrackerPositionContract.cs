using System.Collections.Generic;

using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create portfolio tracker position
    /// </summary>
    public class CreatePortfolioTrackerPositionContract
    {
        /// <summary>
        /// Id of Portfolio this portfolio must belong to.
        /// </summary>
        public int PortfolioId { get; set; }

        /// <summary>
        ///  Position type.
        /// </summary>
        public PositionTypes PositionType { get; set; }

        /// <summary>
        ///  Recommended advice type for position.
        /// </summary>
        public PositionAdviceTypes AdviceType { get; set; }

        /// <summary>
        ///  Additional notes for recommender advice.
        /// </summary>
        public string AdviceNotes { get; set; }

        /// <summary>
        ///  First field for additional notes for position.
        /// </summary>
        public string Notes1 { get; set; }

        /// <summary>
        ///  List of subtrades.
        /// </summary>
        public List<CreatePtSubtradeContract> Subtrades { get; set; }
    }
}
