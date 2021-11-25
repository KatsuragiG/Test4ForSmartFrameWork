using System;
using System.Collections.Generic;
using System.Text;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Portfolio tracker position contract.
    /// </summary>
    public class PortfolioTrackerPositionContract
    {
        /// <summary>
        /// Unique position Id.
        /// </summary>
        public int PositionId { get; set; }

        /// <summary>
        /// Id of Portfolio this portfolio belongs to.
        /// </summary>
        public int PortfolioId { get; set; }

        /// <summary>
        ///  Position type.
        /// </summary>
        public PositionTypes PositionType { get; set; }

        /// <summary>
        /// Overal Position status. Open - has opened subtrades, Closed - all subtrades are closed.
        /// </summary>
        public PositionStatusTypes StatusType { get; set; }

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
        /// Indicates whether this position is published in TradeSmith Gurus center or not.
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// Indicates wheither position was manually deleted by user.
        /// </summary>
        public bool Delisted { get; set; }

        /// <summary>
        ///  List of subtrades.
        /// </summary>
        public List<PtSubtradeContract> Subtrades { get; set; }
    }
}
