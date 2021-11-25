using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Portfolio tracker subtrade contract.
    /// </summary>
    public class PtSubtradeContract
    {
        /// <summary>
        /// Unique subtrade Id.
        /// </summary>
        public int SubtradeId { get; set; }

        /// <summary>
        /// Id of position this subtrade belongs to.
        /// </summary>
        public int Positionid { get; set; }

        /// <summary>
        /// Symbol contract
        /// </summary>
        public SymbolContract Symbol { get; set; }

        /// <summary>
        /// Subtrade trade type.
        /// </summary>
        public TradeTypes TradeType { get; set; }

        /// <summary>
        /// Subtrade status type.
        /// </summary>
        public PositionStatusTypes StatusType { get; set; }

        /// <summary>
        /// Indicates wheither subtrade was manually deleted by user.
        /// </summary>
        public bool Delisted { get; set; }
    }
}
