using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to edit portfolio tracker subtrade.
    /// </summary>
    public class EditPtSubtradeContract 
    {
        /// <summary>
        /// Subtrade trade type.
        /// </summary>
        public TradeTypes TradeType { get; set; } 
    }
}
